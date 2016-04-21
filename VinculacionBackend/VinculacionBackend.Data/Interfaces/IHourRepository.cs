using VinculacionBackend.Data.Entities;

namespace VinculacionBackend.Data.Interfaces
{
    public interface IHourRepository : IRepository<Hour>
    {
       Hour InsertHourFromModel(string accountId, long sectionId, long projectId, int Hour);
    }
}
