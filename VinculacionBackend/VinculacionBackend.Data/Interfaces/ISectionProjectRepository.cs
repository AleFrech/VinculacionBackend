using System.Linq;
using VinculacionBackend.Data.Entities;

namespace VinculacionBackend.Data.Interfaces
{
    public interface ISectionProjectRepository : IRepository<SectionProject>
    {
        IQueryable<SectionProject> GetUnapprovedProjects();
        SectionProject GetSectionProjectByIds(long sectionId, long projectId);
    }
}
