using System.Linq;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;

namespace VinculacionBackend.Services
{
    public class PeriodsServices : IPeriodsServices

    {
        private readonly IPeriodRepository _periodsRepository;

        public PeriodsServices(IPeriodRepository periodsRepository)
        {
            _periodsRepository = periodsRepository;
        }
        public IQueryable<Period> All()
        {
            return _periodsRepository.GetAll();
        }

        public Period Delete(long id)
        {
            var period = _periodsRepository.Delete(id);
            _periodsRepository.Save();
            return period;
        }

        public void Add(Period period)
        {
            _periodsRepository.Insert(period);
            _periodsRepository.Save();
        }

        public Period Find(long id)
        {
            return _periodsRepository.Get(id);
        }

        public Period Map(PeriodEntryModel periodModel)
        {
            var newPeriod = new Period();
            newPeriod.Number = periodModel.Number;
            newPeriod.Year = periodModel.Year;
            return newPeriod;
        }
    }
}