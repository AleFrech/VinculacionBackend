using VinculacionBackend.Entities;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;

namespace VinculacionBackend.Services
{
    public class HoursServices
    {
        private readonly IHourRepository _hourRepository;

        public HoursServices(IHourRepository hourRepository)
        {
            this._hourRepository = hourRepository;
        }


        public Hour Add(HourEntryModel hourModel)
        {
            var hour = _hourRepository.InsertHourFromModel(hourModel);
            _hourRepository.Save();
            return hour;
        }
    }
}