using System;
using System.Linq;
using VinculacionBackend.Data.Database;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;

namespace VinculacionBackend.Data.Repositories
{
    public class ClassRepository:IClassRepository
    {
        private VinculacionContext _db;

        public ClassRepository()
        {
            _db = new VinculacionContext();
        }
        public IQueryable<Class> GetAll()
        {
            return _db.Classes;
        }

        public Class Get(long id)
        {
            return _db.Classes.Find(id);
        }

        public Class Delete(long id)
        {
            var found = Get(id);
            if (found != null)
                _db.Classes.Remove(found);
            return found;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Class ent)
        {
            _db.Entry(ent).State = System.Data.Entity.EntityState.Modified;
        }

        public void Insert(Class ent)
        {
            _db.Classes.Add(ent);
        }
    }
}
