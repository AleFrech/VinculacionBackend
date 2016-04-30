using System.Data.Entity;
using System.Linq;
using VinculacionBackend.Data.Database;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;

namespace VinculacionBackend.Data.Repositories
{
    public class SectionRepository : ISectionRepository
    {
        private readonly VinculacionContext _db;

        public SectionRepository()
        {
            _db = new VinculacionContext();
        }
        public Section Delete(long id)
        {
            var found = Get(id);
            _db.Sections.Remove(found);
            return found;
        }

        public Section Get(long id)
        {
            return _db.Sections.Include(a => a.Class).Include(b => b.User).Include(c => c.Period).FirstOrDefault(d=>d.Id==id);
        }

        public IQueryable<Section> GetAll()
        {
            return _db.Sections.Include(a => a.Class).Include(b => b.User).Include(c => c.Period);
        }

        public void Insert(Section ent)
        {
            _db.Sections.Add(ent);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Section ent)
        {
            _db.Entry(ent).State = EntityState.Modified;
        }
    }
}