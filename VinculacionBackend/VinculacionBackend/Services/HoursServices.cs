using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;

namespace VinculacionBackend.Services
{
    public class HoursServices : IHoursServices
    {
        private readonly IHourRepository _hourRepository;

        public HoursServices(IHourRepository hourRepository)
        {
            this._hourRepository = hourRepository;
        }

        public Hour Add(HourEntryModel hourModel,string professorUser)
        {
            var hour =_hourRepository.InsertHourFromModel(hourModel.AccountId, hourModel.SectionId, hourModel.ProjectId, hourModel.Hour,professorUser);
            _hourRepository.Save();
            return hour;
        }
    }
}