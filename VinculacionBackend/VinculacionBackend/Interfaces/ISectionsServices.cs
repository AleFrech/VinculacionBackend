using System.Linq;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Models;

namespace VinculacionBackend.Interfaces
{
    public interface ISectionsServices
    {
        IQueryable<Section> All();
        Section Delete(long sectionId);
        void Add(Section section);
        Section Find(long id);
        bool AssignStudents(SectionStudentModel model);
        Section Map(SectionEntryModel sectionModel);
        bool RemoveStudents(SectionStudentModel model);
        Section UpdateSection(long sectionId,SectionEntryModel model);
        IQueryable<User> GetSectionStudents(long sectionId);
        IQueryable<Project> GetSectionsProjects(long sectionId);
    }
}
