using System.Collections.Generic;
using System.Linq;
using VinculacionBackend.Data.Entities;

namespace VinculacionBackend.Data.Interfaces
{
    public interface IProjectRepository : IRepository<Project>
    {
        IQueryable<User> GetProjectStudents(long projectId);
        void Insert(Project ent, List<string> majorIds, List<long> sectionIds);
        void AssignToSection(long projectId, long sectionId);
        SectionProject RemoveFromSection(long projectId, long sectionId);
        IQueryable<Project> GetAllStudent(long userId);
        IQueryable<Project> GetAllProfessor(long userId);
        Section GetSection(Project project);
    }
}
