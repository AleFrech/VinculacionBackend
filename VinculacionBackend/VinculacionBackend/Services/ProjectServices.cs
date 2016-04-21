using System.Linq;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;

namespace VinculacionBackend.Services
{
    public class ProjectServices
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectServices(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public Project Find(long id)
        {
            return _projectRepository.Get(id);
        }

        public IQueryable<Project> All()
        {
            return _projectRepository.GetAll();
        }

        private Project Map(ProjectModel model )
        {
            var newProject= new Project();
            newProject.Name = model.Name;
            newProject.Description = model.Description;
            return newProject;
        }

        public void Add(ProjectModel model)
        {
            var project = Map(model);
            _projectRepository.Insert(project);
            _projectRepository.Save();
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
            if (tmpProject == null) {
                return null;
            }

            tmpProject.Name = model.Name;
            tmpProject.Description = model.Description;
            _projectRepository.Update(tmpProject);
            _projectRepository.Save();
            return tmpProject;
        }
    }
}