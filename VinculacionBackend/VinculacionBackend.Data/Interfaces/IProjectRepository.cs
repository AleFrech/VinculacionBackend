using System.Collections.Generic;
using System.Linq;
using VinculacionBackend.Data.Entities;

namespace VinculacionBackend.Data.Interfaces
{
    public interface IProjectRepository : IRepository<Project>
    {
        IQueryable<User> GetProjectStudents(long projectId);
        void Insert(Project ent, List<string> majorIds);
        void AssignToSection(Project ent, long sectionId);
    }
}
