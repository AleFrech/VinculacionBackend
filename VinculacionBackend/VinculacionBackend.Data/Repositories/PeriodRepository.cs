using System.Linq;
using VinculacionBackend.Data.Database;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;

namespace VinculacionBackend.Data.Repositories
{
    public class PeriodRepository : IPeriodRepository
    {
        private VinculacionContext _db;

        public PeriodRepository()
        {
            _db = new VinculacionContext();
        }
        public IQueryable<Period> GetAll()
        {
            return _db.Periods;
        }

        public Period Get(long id)
        {
            return _db.Periods.Find(id);
        }

        public Period Delete(long id)
        {
            var found = Get(id);
            if (found != null)
                _db.Periods.Remove(found);
            return found;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Period ent)
        {
            _db.Entry(ent).State = System.Data.Entity.EntityState.Modified;
        }

        public void Insert(Period ent)
        {
            _db.Periods.Add(ent);
        }

        public Period GetCurrent()
        {
            return _db.Periods.FirstOrDefault(x =>x.IsCurrent);
        }
    }
}