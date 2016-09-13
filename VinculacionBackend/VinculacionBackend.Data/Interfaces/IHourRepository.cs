using System.Linq;
using VinculacionBackend.Data.Entities;

namespace VinculacionBackend.Data.Interfaces
{
    public interface IHourRepository : IRepository<Hour>
    {
       Hour InsertHourFromModel(string accountId, long sectionId, long projectId, int hour,string professorUser, bool isAdmin);
       IQueryable<Hour> GetStudentHours(string accountId);
        SectionProject GetSectionProjectRel(long sectionProjectId);
        void Update(SectionProject ent);
    }
}
