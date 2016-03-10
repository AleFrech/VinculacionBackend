using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VinculacionBackend.Database;
using VinculacionBackend.Entities;
using System.Data.Entity;

namespace VinculacionBackend
{
    public class SectionRepository : ISectionRepository
    {
        private VinculacionContext db;

        public SectionRepository()
        {
            db = new VinculacionContext();
        }
        public void Delete(long id)
        {
            var found = Get(id);
            db.Sections.Remove(found);
        }

        public Section Get(long id)
        {
            return db.Sections.Include(a => a.Class).Include(b => b.User).Include(c => c.Period).FirstOrDefault(d=>d.Id==id);
        }

        public IEnumerable<Section> GetAll()
        {
            return db.Sections.Include(a => a.Class).Include(b => b.User).Include(c => c.Period);
        }

        public void Insert(Section ent)
        {
            db.Sections.Add(ent);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(Section ent)
        {
            db.Entry(ent).State = EntityState.Modified;
        }
    }
}