using VinculacionBackend.Data.Entities;

namespace VinculacionBackend.Data.Interfaces
{
    public interface ISectionProjectRepository : IRepository<SectionProject>
    {
        SectionProject GetSectionProjectByIds(long sectionId, long projectId);
    }
}
