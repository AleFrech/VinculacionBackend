using VinculacionBackend.Entities;

namespace VinculacionBackend.Repositories
{
    interface IProjectRepository : IRepository<Project>
    {
        IEnumerable<User> GetProjectStudents(long projectId);
    }
}
