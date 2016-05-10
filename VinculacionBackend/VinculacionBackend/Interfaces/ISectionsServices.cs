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
        bool AssignStudent(SectionStudentModel model);
        Section Map(SectionEntryModel sectionModel);
        bool RemoveStudent(SectionStudentModel model);
    }
}
