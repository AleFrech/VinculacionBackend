using System.Linq;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Exceptions;
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

        public void Map(Period period,PeriodEntryModel periodModel)
        {
            period.Number = periodModel.Number;
            period.Year = periodModel.Year;
            period.FromDate = periodModel.FromDate;
            period.ToDate = periodModel.ToDate;
            period.IsCurrent = false;
        }


        public Period UpdatePeriod(long preriodId, PeriodEntryModel model)
        {
            var period = _periodsRepository.Get(preriodId);
            if (period == null)
                throw new NotFoundException("No se encontro el periodo");
            Map(period, model);
            _periodsRepository.Update(period);
            _periodsRepository.Save();
            return period;
        }

        public Period SetCurrentPeriod(long periodId)
        {
            var period = _periodsRepository.Get(periodId);
            if (period == null)
                throw new NotFoundException("No se encontro el periodo");
            var periods = _periodsRepository.GetAll();
            foreach (var p in periods)
            {
                p.IsCurrent = p.Id == period.Id;
                _periodsRepository.Update(p);
            }
            _periodsRepository.Save();
            return period;
        }

        public Period GetCurrentPeriod()
        {
            return _periodsRepository.GetCurrent();
        }
    }
}