using System.Linq;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Exceptions;
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
            if(period == null)
                throw new NotFoundException("No se encontro el periodo");
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
            var period = _periodsRepository.Get(id);
            if(period==null)
                throw new NotFoundException("No se encontro el periodo");
            return period;
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