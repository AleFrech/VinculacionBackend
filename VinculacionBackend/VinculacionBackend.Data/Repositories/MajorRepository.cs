using System.Data.Entity;
using System.Data.Entity.Migrations;
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
        public IQueryable<Major> GetMajorsByFaculty(long facultyId)
        {
            return _db.Majors.Include(major => major.Faculty).Where(major => major.Faculty.Id == facultyId);
        }

        public IQueryable<Major> GetMajorsByProject(long projectId)
        {
            return _db.ProjectMajorRels.Include(rel => rel.Project).Include(rel => rel.Major).Where(rel => rel.Project.Id == projectId).Select(rel => rel.Major).Include(x=>x.Faculty);
        }

        public Major GetMajorByName(string name)
        {
            return _db.Majors.FirstOrDefault(major => major.Name == name);
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
            
            return _db.Majors.Include(p => p.Faculty).FirstOrDefault(x=>x.Id == id);
        }

  

        public IQueryable<Major> GetAll()
        {
            return _db.Majors.Include(a => a.Faculty);
        }

        public Major GetMajorByMajorId(string majorId)
        {
            return _db.Majors.Include(a => a.Faculty).FirstOrDefault(x => x.MajorId == majorId);
        }

        public void Insert(Major ent)
        {
            _db.Faculties.Attach(ent.Faculty);
            _db.Majors.Add(ent);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Major ent)
        {
            _db.Faculties.Attach(ent.Faculty);
            _db.Majors.AddOrUpdate(ent);
        }
    }
}