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
        Section Map(SectionEntryModel sectionModel);
    }
}
