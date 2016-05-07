using System.Linq;
using VinculacionBackend.Data.Entities;

namespace VinculacionBackend.Data.Interfaces
{
    public interface IHourRepository : IRepository<Hour>
    {
       Hour InsertHourFromModel(string accountId, long sectionId, long projectId, int hour,string professorUser);
       IQueryable<Hour> GetStudentHours(string accountId);
    }
}
