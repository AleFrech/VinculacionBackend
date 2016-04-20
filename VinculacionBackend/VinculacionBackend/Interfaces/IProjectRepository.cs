using System.Linq;
using VinculacionBackend.Entities;
using VinculacionBackend.Repositories;

namespace VinculacionBackend.Interfaces
{
    public interface IProjectRepository : IRepository<Project>
    {
        IQueryable<User> GetProjectStudents(long projectId);
    }
}
