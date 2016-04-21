using VinculacionBackend.Data.Entities;
using VinculacionBackend.Models;

namespace VinculacionBackend.Services
{
    public interface IHoursServices
    {
        Hour Add(HourEntryModel hourModel);
    }
}