using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;
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
            _hourRepository.InsertHourFromModel(hourModel);
            _hourRepository.Save();
            return null;
        }
    }
}