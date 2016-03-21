using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VinculacionBackend.Entities;
using VinculacionBackend.Repositories;

namespace VinculacionBackend.Services
{
    public class ProjectServices
    {
        readonly IProjectRepository _projectRepository = new ProjectRepository();
        readonly IStudentRepository _studentRepository = new StudentRepository();
        public Project Find(long id)
        {
            return _projectRepository.Get(id);
        }

        public IEnumerable<Project> All()
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

        public IEnumerable<User> GetProjectStudents(long projectId)
        {
            return _projectRepository.GetProjectStudents(projectId);
        }
    }
}