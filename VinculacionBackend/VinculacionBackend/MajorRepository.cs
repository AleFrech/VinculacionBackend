using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VinculacionBackend.Database;
using VinculacionBackend.Entities;
using System.Data.Entity;

namespace VinculacionBackend
{
    public class MajorRepository : IMajorRepository
    {
        private VinculacionContext db;
        public MajorRepository()
        {
            db = new VinculacionContext();
        }
        public void Delete(long id)
        {
            var found = Get(id);
            db.Majors.Remove(found);
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