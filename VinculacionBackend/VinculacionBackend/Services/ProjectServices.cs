using System.Linq;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;

namespace VinculacionBackend.Services
{
    public class ProjectServices : IProjectServices
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ISectionRepository _sectionRepository;

        public ProjectServices(IProjectRepository projectRepository, ISectionRepository sectionRepository)
        {
            _projectRepository = projectRepository;
            _sectionRepository = sectionRepository;
        }

        public Project Find(long id)
        {
            return _projectRepository.Get(id);
        }

        public IQueryable<Project> All()
        {
            return _projectRepository.GetAll();
        }


        private Project Map(ProjectModel model)
        {
            var newProject = new Project();
            newProject.ProjectId = model.ProjectId;
            newProject.Name = model.Name;
            newProject.Description = model.Description;
            newProject.Cost = model.Cost;
            newProject.BeneficiariesAlias = model.BenificiariesAlias;
            newProject.BeneficiariesQuantity = model.BenificiariesQuantity;
            return newProject;
        }

        public Project Add(ProjectModel model)
        {
            var project = Map(model);
            _projectRepository.Insert(project, model.MajorIds);
            _projectRepository.Save();
            return project;
        }

        public Project Delete(long projectId)
        {
            var project = _projectRepository.Delete(projectId);
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
            {
                return null;
            }
            tmpProject.ProjectId = model.ProjectId;
            tmpProject.Name = model.Name;
            tmpProject.Description = model.Description;
            tmpProject.Cost = model.Cost;
            tmpProject.BeneficiariesAlias = model.BenificiariesAlias;
            tmpProject.BeneficiariesQuantity = model.BenificiariesQuantity;
            _projectRepository.Update(tmpProject);
            _projectRepository.Save();
            return tmpProject;
        }

        public bool AssignSection(ProjectSectionModel model)
        {
            var project = _projectRepository.Get(model.ProjectId);
            var section = _sectionRepository.Get(model.SectionId);

            if (project == null || section == null) return false;

            _projectRepository.AssignToSection(model.ProjectId, model.SectionId);
            _projectRepository.Save();
            return true;
        }
    }

}