using System.Data.Entity;
using System.Linq;
using VinculacionBackend.Data.Database;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;

namespace VinculacionBackend.Data.Repositories
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
            var rels = db.ProjectMajorRels.Where(x => x.Major.Id == found.Id);
            foreach (var rel in rels)
            {
                db.ProjectMajorRels.Remove(rel);
            }
            db.Majors.Remove(found);

            return found;
        }

        public Major Get(long id)
        {
            db.Majors.Include(x => x.Id
            );
            return db.Majors.Find(id);
        }

        public IQueryable<Major> GetAll()
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