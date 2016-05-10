using System;
using System.Data.Entity;
using System.Linq;
using VinculacionBackend.Data.Database;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;

namespace VinculacionBackend.Data.Repositories
{
    public class SectionRepository : ISectionRepository
    {
        private VinculacionContext db;

        public SectionRepository()
        {
            db = new VinculacionContext();
        }

        public void AssignStudent(long sectionId, long studentId)
        {
            var section = Get(sectionId);
            var student = db.Users.FirstOrDefault(x => x.Id == studentId);

            db.SectionUserRels.Add(new SectionUser { Section = section, User = student });
        }
        
        public void RemoveStudent(long sectionId, long studentId)
        {
            var found = db.SectionUserRels.FirstOrDefault(x=>x.sectionId == sectionId && x.studentId == studentId);
            db.SectionUserRels.Remove(found);
        }

        public Section Delete(long id)
        {
            var found = Get(id);
            db.Sections.Remove(found);
            return found;
        }

        public Section Get(long id)
        {
            return db.Sections.Include(a => a.Class).Include(b => b.User).Include(c => c.Period).FirstOrDefault(d=>d.Id==id);
        }

        public IQueryable<Section> GetAll()
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