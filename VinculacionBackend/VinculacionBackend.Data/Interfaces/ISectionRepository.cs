using System.Collections.Generic;
using System.Linq;
using VinculacionBackend.Data.Entities;

namespace VinculacionBackend.Data.Interfaces
{
    public interface ISectionRepository : IRepository<Section>
    {
        void AssignStudents(long sectionId, List<string> studenstIds);
        void RemoveStudents(long sectionId, List<string> studentsIds);
        IQueryable<User> GetSectionStudents(long sectionId);
        IQueryable<Project> GetSectionProjects(long sectionId);
        IQueryable<Section> GetAllByStudent(long userId);
        IQueryable<Section> GetSectionsByProject(long projectId);
        IQueryable<Section> GetAllByProfessor(long userId);
        IQueryable<SectionUser> GetSectionsUsersRels();
        object GetSectionStudentsHours(long sectionId, long projectId);
        void ClearSectionStudents(long sectionId);
    }
}
