using System.Linq;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Models;

namespace VinculacionBackend.Interfaces
{
    public interface IPeriodsServices
    {
        IQueryable<Period> All();
        Period Delete(long id);
        void Add(Period period);
        Period Find(long id);
        void Map(Period period,PeriodEntryModel periodModel);
        Period UpdatePeriod(long preriodId, PeriodEntryModel model);
        Period SetCurrentPeriod(long periodId);
        Period GetCurrentPeriod();
    }
}