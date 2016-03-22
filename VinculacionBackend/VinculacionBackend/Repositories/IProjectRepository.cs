using System.Collections.Generic;
using System.Linq;
using VinculacionBackend.Entities;

namespace VinculacionBackend.Repositories
{
    interface IProjectRepository : IRepository<Project>
    {
        IQueryable<User> GetProjectStudents(long projectId);
    }
}
