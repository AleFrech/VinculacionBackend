using VinculacionBackend.Entities;
using VinculacionBackend.Models;

namespace VinculacionBackend.Repositories
{
    public interface IHourRepository : IRepository<Hour> 
    {
        Hour InsertHourFromModel(HourEntryModel model);
    }
}
