using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Data.Repositories;
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
        private readonly IPeriodRepository _periodRepository;
        private readonly ISectionProjectRepository _sectionProjectRepository;
        List<int> _periods = new List<int>();

        public ProjectServices(IProjectRepository projectRepository, ISectionRepository sectionRepository,
            IStudentRepository studentRepository, ITextDocumentServices textDocumentServices, IMajorRepository majorRepository, IClassRepository classRepository, IPeriodRepository periodRepository, ISectionProjectRepository sectionProjectRepository)
        {
            _projectRepository = projectRepository;
            _sectionRepository = sectionRepository;
            _studentRepository = studentRepository;
            _textDocumentServices = textDocumentServices;
            _majorRepository = majorRepository;
            _classRepository = classRepository;
            _periodRepository = periodRepository;
            _sectionProjectRepository = sectionProjectRepository;
            _periods.Add(1);
            _periods.Add(2);
            _periods.Add(3);
            _periods.Add(5);
        }

        public ProjectServices(IProjectRepository projectRepository, IMajorRepository majorRepository, IClassRepository classRepository, IPeriodRepository periodRepository, ISectionProjectRepository sectionProjectRepository)
        {
            _projectRepository = projectRepository;
            _majorRepository = majorRepository;
            _classRepository = classRepository;
            _periodRepository = periodRepository;
            _sectionProjectRepository = sectionProjectRepository;
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


        public int GetProjectsTotalByMajor(Major major)
        {
            var currentPeriod = _periodRepository.GetCurrent();
            var majorProjectTotalmodels = _projectRepository.GetMajorProjectTotal(currentPeriod, major.MajorId);
            return majorProjectTotalmodels.Sum(x => x.Total);
        }


        private void Map(Project project, ProjectModel model)
        {
            project.ProjectId = model.ProjectId;
            project.Name = model.Name;
            project.Description = model.Description;
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


        public void AssignProjectsToSection(ProjectsSectionModel model)
        {
            foreach (var p in model.ProjectIds)
            {
                _projectRepository.AssignToSection(p,model.SectionId);
            }
            _projectRepository.Save();
        }

        public IQueryable<Project> GetProjectsBySection(long sectionId)
        {
            return _projectRepository.GetProjectsBySection(sectionId);
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



        public HttpResponseMessage GetFinalReport(long projectId,long sectionId, int fieldHours, int calification,
            int beneficiariesQuantities, string beneficiariGroups)
        {
            var sp = _projectRepository.GetSectionProject(projectId, sectionId);
            if (sp.IsApproved)
                throw new Exception("Las horas de este proyecto ya fueron approvadas");

            var finalReport = new ProjectFinalReport(_projectRepository, _sectionRepository, _studentRepository,
                _textDocumentServices, new DownloadbleFile(),_sectionProjectRepository);
            return finalReport.GenerateFinalReport(projectId,sp.Id,fieldHours, calification, beneficiariesQuantities,
                beneficiariGroups);

        }

        public DataTable CreateProjectsByMajor()
        {
            var dt = new DataTable();
            dt.Columns.Add("Carrera", typeof(string));
            dt.Columns.Add("Proyectos", typeof(int));

            var majors = _majorRepository.GetAll().ToList();
            foreach (var m in majors)
            {
                var totalProjectsByMajor = GetProjectsTotalByMajor(m);
                 dt.Rows.Add(m.Name,totalProjectsByMajor);                
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
                dt.Rows.Add(project.ProjectId, project.Name, _projectRepository.GetTotalCostByProject(project.Id), project.BeneficiarieOrganization, professors,
                    period.Number, period.Year);
            }

            return dt;
        }

        public IQueryable<PeriodReportModel> CreatePeriodReport(int year, int period)
        {
            return _projectRepository.GetByYearAndPeriod(year, period);
        }
    }

}