using VinculacionBackend.Entities;
using VinculacionBackend.Models;

namespace VinculacionBackend.Repositories
{
    interface IHourRepository : IRepository<Hour> 
    {
        Hour InsertHourFromModel(HourEntryModel model);
    }
}
