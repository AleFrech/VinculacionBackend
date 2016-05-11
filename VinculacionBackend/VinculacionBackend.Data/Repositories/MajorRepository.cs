using System.Data.Entity;
using System.Linq;
using VinculacionBackend.Data.Database;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;

namespace VinculacionBackend.Data.Repositories
{
    public class MajorRepository : IMajorRepository
    {
        private readonly VinculacionContext _db;
        public MajorRepository()
        {
            _db = new VinculacionContext();
        }
        public Major Delete(long id)
        {
            var found = Get(id);
            var rels = _db.ProjectMajorRels.Where(x => x.Major.Id == found.Id);
            foreach (var rel in rels)
            {
                _db.ProjectMajorRels.Remove(rel);
            }
            _db.Majors.Remove(found);

            return found;
        }

        public Major Get(long id)
        {
            _db.Majors.Include(x => x.Id
            );
            return _db.Majors.Find(id);
        }

        public IQueryable<Major> GetAll()
        {
            return _db.Majors;
        }

        public Major GetMajorByMajorId(string majorId)
        {
            return _db.Majors.FirstOrDefault(x => x.MajorId == majorId);
        }

        public void Insert(Major ent)
        {
            _db.Majors.Add(ent);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Major ent)
        {
            _db.Entry(ent).State = System.Data.Entity.EntityState.Modified;
        }
    }
}