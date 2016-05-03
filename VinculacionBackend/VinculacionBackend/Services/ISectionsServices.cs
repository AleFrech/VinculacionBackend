using System.Linq;
using VinculacionBackend.Data.Entities;

namespace VinculacionBackend.Services
{
    public interface ISectionsServices
    {
        IQueryable<Section> All();
        Section Delete(long sectionId);
        void Add(Section section);
        Section Find(long id);
    }
}
