using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Web.WebPages;
using DocumentFormat.OpenXml.Drawing.Charts;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Data.Models;
using VinculacionBackend.Exceptions;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;
using VinculacionBackend.Reports;
using DataTable = System.Data.DataTable;

namespace VinculacionBackend.Services
{
    public class ProjectServices : IProjectServices
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ISectionRepository _sectionRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ITextDocumentServices _textDocumentServices;
        private readonly IMajorRepository _majorRepository;
        private readonly IClassRepository _classRepository;
        List<int> _periods = new List<int>();

        public ProjectServices(IProjectRepository projectRepository, ISectionRepository sectionRepository,
            IStudentRepository studentRepository, ITextDocumentServices textDocumentServices, IMajorRepository majorRepository, IClassRepository classRepository)
        {
            _projectRepository = projectRepository;
            _sectionRepository = sectionRepository;
            _studentRepository = studentRepository;
            _textDocumentServices = textDocumentServices;
            _majorRepository = majorRepository;
            _classRepository = classRepository;
            _periods.Add(1);
            _periods.Add(2);
            _periods.Add(3);
            _periods.Add(5);
        }

        public ProjectServices(IProjectRepository projectRepository, IMajorRepository majorRepository, IClassRepository classRepository)
        {
            _projectRepository = projectRepository;
            _majorRepository = majorRepository;
            _classRepository = classRepository;
        }

        public Project Find(long id)
        {
            var project = _projectRepository.Get(id);
            if (project == null)
                throw new NotFoundException("No se encontro el proyecto");
            return project;
        }

        public IQueryable<Project> All()
        {
            return _projectRepository.GetAll();
        }


        public Dictionary<string, List<PeriodProjectsModel>> GetProjectsTotalByMajor(int year, Major major)
        {
            Dictionary<string, List<PeriodProjectsModel>> reportDictionary =
                new Dictionary<string, List<PeriodProjectsModel>>();
            List<PeriodProjectsModel> periodProjects = new List<PeriodProjectsModel>();

            periodProjects.Add(new PeriodProjectsModel
            {
                Period = 0,
                TotalProjects = 0
            });
            periodProjects.Add(new PeriodProjectsModel
            {
                Period = 0,
                TotalProjects = 0
            });
            periodProjects.Add(new PeriodProjectsModel
            {
                Period = 0,
                TotalProjects = 0
            });
            periodProjects.Add(new PeriodProjectsModel
            {
                Period = 0,
                TotalProjects = 0
            });

            var majorProjectTotalmodels = new List<MajorProjectTotalmodel>();

            for (var i = 0; i < 4; i++)
            {
                 majorProjectTotalmodels  = _projectRepository.GetMajorProjectTotal(i, year, major.MajorId);
                if (majorProjectTotalmodels.Count > 0)
                {
                    var total = majorProjectTotalmodels.Sum(x => x.Total);
                    periodProjects.ElementAt(i).Period = _periods.ElementAt(i);
                    periodProjects.ElementAt(i).TotalProjects = total;
                }
            }
          //  if (majorProjectTotalmodels.Count > 0)
           // {
                reportDictionary.Add(major.Name, periodProjects);
           // }

            return reportDictionary;
        }


        private void Map(Project project, ProjectModel model)
        {
            project.ProjectId = model.ProjectId;
            project.Name = model.Name;
            project.Description = model.Description;
            project.Cost = model.Cost;
            project.MajorIds = model.MajorIds;
            project.SectionIds = model.SectionIds;
            project.BeneficiarieOrganization = model.BeneficiarieOrganization;
        }

        public Project Add(ProjectModel model)
        {

            var project = new Project();
            Map(project, model);
            _projectRepository.Insert(project, model.MajorIds, model.SectionIds);
            _projectRepository.Save();
            return project;
        }

        public Project Delete(long projectId)
        {
            var project = _projectRepository.Delete(projectId);
            if (project == null)
                throw new NotFoundException("No se encontro el proyecto");
            _projectRepository.Save();
            return project;
        }

        public IQueryable<User> GetProjectStudents(long projectId)
        {
            return _projectRepository.GetProjectStudents(projectId);
        }

        public Project UpdateProject(long projectId, ProjectModel model)
        {
            var tmpProject = _projectRepository.Get(projectId);
            if (tmpProject == null)
                throw new NotFoundException("No se encontro el proyecto");
            Map(tmpProject, model);
            _projectRepository.Update(tmpProject);
            _projectRepository.Save();
            return tmpProject;
        }

        public bool AssignSection(ProjectSectionModel model)
        {
            _projectRepository.AssignToSection(model.ProjectId, model.SectionId);
            _projectRepository.Save();
            return true;
        }


        public bool RemoveFromSection(long projectId, long sectionId)
        {
            var rel = _projectRepository.RemoveFromSection(projectId, sectionId);

            if (rel == null)
            {
                throw new NotFoundException("Seccion o Proyecto invalido");
            }

            return true;
        }

        public IQueryable<Project> GetUserProjects(long userId, string[] roles)
        {
            if (roles.Contains("Admin"))
            {
                return _projectRepository.GetAll();
            }
            else if (roles.Contains("Professor"))
            {
                return _projectRepository.GetAllProfessor(userId);
            }
            else if (roles.Contains("Student"))
            {
                return _projectRepository.GetAllStudent(userId);
            }
            throw new Exception("No tiene permiso");

        }



        public HttpResponseMessage GetFinalReport(long projectId, int fieldHours, int calification,
            int beneficiariesQuantities, string beneficiariGroups)
        {
            var finalReport = new ProjectFinalReport(_projectRepository, _sectionRepository, _studentRepository,
                _textDocumentServices, new DownloadbleFile());
            return finalReport.GenerateFinalReport(projectId, fieldHours, calification, beneficiariesQuantities,
                beneficiariGroups);

        }

        public DataTable CreateProjectsByMajor(int year)
        {
            var dt = new DataTable();
            dt.Columns.Add("Carrera", typeof(string));
            dt.Columns.Add("Periodo 1", typeof(int));
            dt.Columns.Add("Periodo 2", typeof(int));
            dt.Columns.Add("Periodo 3", typeof(int));
            dt.Columns.Add("Periodo 5", typeof(int));

            var majors = _majorRepository.GetAll().ToList();

            foreach (var m in majors)
            {
                var projectByMajor = GetProjectsTotalByMajor(year, m);
                foreach (var key in projectByMajor.Keys)
                {
                    dt.Rows.Add(key, projectByMajor[key].ElementAt(0).TotalProjects, projectByMajor[key].ElementAt(1).TotalProjects
                    , projectByMajor[key].ElementAt(2).TotalProjects, projectByMajor[key].ElementAt(3).TotalProjects);
                }
            }

            return dt;
        }


        public DataTable ProjectsByClass(long classId)
        {
            var @class = _classRepository.Get(classId);
            Object[] titleRow = {"Clase: "+@class.Name};
            var dt = new DataTable();
           // dt.Rows.Add(titleRow);
            dt.Columns.Add("Id Proyecto", typeof(string));
            dt.Columns.Add("Nombre", typeof(string));
            dt.Columns.Add("Costo", typeof(double));
            dt.Columns.Add("Beneficiario", typeof(string));
            dt.Columns.Add("Maestros", typeof(string));
            dt.Columns.Add("Periodo", typeof(int));
            dt.Columns.Add("Anio", typeof(int));

            var projects = _projectRepository.GetProjectsByClass(classId).ToList();
            foreach (var project in projects)
            {
                var professorsList =_projectRepository.GetProfessorsByProject(project.Id).Select(x => x.Name).Distinct().ToList();
                var professors = professorsList.Count>0 ? string.Join(",", _projectRepository.GetProfessorsByProject(project.Id).Select(x=>x.Name).Distinct().ToList()):"";
                var period = _projectRepository.GetPeriodByProject(project.Id);
                dt.Rows.Add(project.ProjectId, project.Name, project.Cost, project.BeneficiarieOrganization, professors,
                    period.Number, period.Year);
            }

            return dt;
        }

        public DataTable CreatePeriodReport(int year, int period)
        {
            var dt = new DataTable();
            dt.Columns.Add("Institución", typeof(string));
            dt.Columns.Add("Producto", typeof(string));
            dt.Columns.Add("Asignatura", typeof(string));
            dt.Columns.Add("Carrera", typeof(string));
            dt.Columns.Add("Catedrático", typeof(string));
            dt.Columns.Add("Horas", typeof(string));
            dt.Columns.Add("Fecha de Entrega", typeof(string));
            dt.Columns.Add("Costo", typeof(double));
            dt.Columns.Add("# Proy", typeof(long));
            dt.Columns.Add("Beneficiarios", typeof(string));
            dt.Columns.Add("Comentarios", typeof(string));

            var projects = _projectRepository.GetByYearAndPeriod(year, period);

            foreach (var project in projects)
            {
                dt.Rows.Add(project.BeneficiarieOrganization, project.Description,
                    project.SectionIds.Count > 0 ? _projectRepository.getClass(project.SectionIds[0]) : "", 
                    _projectRepository.getMajors(project.MajorIds), 
                    _projectRepository.getProfessor(project.Id),  
                    _projectRepository.getTotalHours(project.Id),
                    "",
                    project.Cost,
                    project.Id);
            }

            return dt;
        }
    }

}