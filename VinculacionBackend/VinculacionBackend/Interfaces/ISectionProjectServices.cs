using System.Linq;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Models;

namespace VinculacionBackend.Interfaces
{
    public interface ISectionProjectServices
    {
        SectionProjectInfoModel GetInfo(long id);
        void Approve(long sectionProjectId);
        IQueryable<SectionProject> GetUnapproved();
    }
}
