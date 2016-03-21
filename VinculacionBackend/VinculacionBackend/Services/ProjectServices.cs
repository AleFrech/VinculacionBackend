using System.Linq;
using VinculacionBackend.Entities;
using VinculacionBackend.Repositories;
using VinculacionBackend.Models;

namespace VinculacionBackend.Services
{
    public class ProjectServices
    {
        readonly IProjectRepository _projectRepository = new ProjectRepository();
        public Project Find(long id)
        {
            return _projectRepository.Get(id);
        }

        public IQueryable<Project> All()
        {
            return _projectRepository.GetAll();
        }

        public void Add(Project project)
        {
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
        
        public IHttpActionResult UpdateProject(long projectId, ProjectModel model) 
        {
            var tmpProject = _projectRepository.Get(projectId);
            if (tmpProject != null)
            {
                tmpProject.Name = model.Name;
                tmpProject.Description = model.Description;
            }
            else
            {
                return NotFound();
            }

            try
            {
                _projectRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
               return InternalServerError(new DbUpdateConcurrencyException());
            }
            return Ok(tmpProject);
        }
    }
}