using VinculacionBackend.Data.Entities;
using VinculacionBackend.Models;

namespace VinculacionBackend.Interfaces
{
    public interface ISectionProjectServices
    {
        SectionProject GetInfo(long sectionId,long projectId);
        void Approve(long sectionId,long projectId);
        SectionProject AddOrUpdate(SectionProjectEntryModel sectionProjectEntryModel);
    }
}
