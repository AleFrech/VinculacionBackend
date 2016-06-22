using System;
using System.Linq;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Exceptions;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;

namespace VinculacionBackend.Services
{
    public class ProjectServices : IProjectServices
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectServices(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public Project Find(long id)
        {
            var project = _projectRepository.Get(id);
            if (project==null)
                throw new NotFoundException("No se encontro el proyecto");
            return project;
        }

        public IQueryable<Project> All()
        {
            return _projectRepository.GetAll();
        }


        private void Map(Project project,ProjectModel model)
        {
            project.ProjectId = model.ProjectId;
            project.Name = model.Name;
            project.Description = model.Description;
            project.Cost = model.Cost;
            project.MajorIds = model.MajorIds;
            project.SectionIds = model.SectionIds;
            project.BeneficiarieOrganization = model.BeneficiarieOrganization;
            project.BeneficiarieGroups = model.BeneficiarieGroups;
            project.BeneficiariesQuantity = model.BeneficiariesQuantity;
        }

        public Project Add(ProjectModel model)
        {
           
            var project = new Project();
            Map(project,model);
            _projectRepository.Insert(project, model.MajorIds,model.SectionIds);
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
             Map(tmpProject,model);
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

        public void CostsReport()
        {
            
        }
    }

}