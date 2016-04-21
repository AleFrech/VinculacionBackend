using System.Linq;
using VinculacionBackend.Entities;
using VinculacionBackend.Models;

namespace VinculacionBackend.Services
{
    public interface IProjectServices
    {
        Project Find(long id);
        IQueryable<Project> All();
        void Add(Project project);
        Project Delete(long projectId);
        IQueryable<User> GetProjectStudents(long projectId);
        Project UpdateProject(long projectId, ProjectModel model);
    }
}