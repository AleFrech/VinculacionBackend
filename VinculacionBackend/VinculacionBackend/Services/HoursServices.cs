using VinculacionBackend.Entities;
using VinculacionBackend.Models;
using VinculacionBackend.Repositories;

namespace VinculacionBackend.Services
{
    public class HoursServices
    {
        readonly HourRepository _hourRepository = new HourRepository();

        public Hour Add(HourEntryModel hourModel)
        {
            var hour = _hourRepository.InsertHourFromModel(hourModel);
            _hourRepository.Save();
            return hour;
        }
    }
}