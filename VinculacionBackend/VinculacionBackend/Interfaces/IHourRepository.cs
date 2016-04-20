using VinculacionBackend.Entities;
using VinculacionBackend.Models;
using VinculacionBackend.Repositories;

namespace VinculacionBackend.Interfaces
{
    public interface IHourRepository : IRepository<Hour> 
    {
        Hour InsertHourFromModel(HourEntryModel model);
    }
}
