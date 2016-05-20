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
        private readonly VinculacionContext _db;

        public SectionRepository()
        {
            _db = new VinculacionContext();
        }

        public void AssignStudent(long sectionId, long studentId)
        {
            var section = Get(sectionId);
            var student = _db.Users.FirstOrDefault(x => x.Id == studentId);

            _db.SectionUserRels.Add(new SectionUser { Section = section, User = student });
        }
        
        public void RemoveStudent(long sectionId, long studentId)
        {
            var found = _db.SectionUserRels.FirstOrDefault(x=>x.Section.Id == sectionId && x.User.Id == studentId);
            _db.SectionUserRels.Remove(found);
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

        public IQueryable<User> GetSectionStudents(long sectionId)
        {
            var secProjRel = _db.SectionProjectsRels.Include(a => a.Section).Where(c => c.Section.Id == sectionId);
            var horas = _db.Hours.Include(a => a.SectionProject).Include(b => b.User).Where(c => secProjRel.Any(d => d.Id == c.SectionProject.Id));
            var users = _db.Users.Include(a => a.Major).Where(b => horas.Any(c => c.User.Id == b.Id));
            return users;
        }

        public IQueryable<Section> GetAll()
        {
            return _db.Sections.Include(a => a.Class).Include(b => b.User).Include(c => c.Period);
        }

        public void Insert(Section ent)
        {
            _db.Classes.Attach(ent.Class);
            _db.Periods.Attach(ent.Period);
            _db.Users.Attach(ent.User);
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