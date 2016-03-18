using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using VinculacionBackend.Database;
using VinculacionBackend.Entities;

namespace VinculacionBackend.Repositories
{
    public class MajorRepository : IMajorRepository
    {
        private VinculacionContext db;
        public MajorRepository()
        {
            db = new VinculacionContext();
        }
        public Major Delete(long id)
        {
            var found = Get(id);
            db.Majors.Remove(found);
            return found;
        }

        public Major Get(long id)
        {
            db.Majors.Include(x => x.Id
            );
            return db.Majors.Find(id);
        }

        public IEnumerable<Major> GetAll()
        {
            return db.Majors;
        }

        public Major GetMajorByMajorId(string majorId)
        {
            return db.Majors.FirstOrDefault(x => x.MajorId == majorId);
        }

        public void Insert(Major ent)
        {
            db.Majors.Add(ent);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(Major ent)
        {
            db.Entry(ent).State = System.Data.Entity.EntityState.Modified;
        }
    }
}