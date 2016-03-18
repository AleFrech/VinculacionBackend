using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using VinculacionBackend.Database;
using VinculacionBackend.Entities;
using VinculacionBackend.Models;

namespace VinculacionBackend.Repositories
{
    public class HourRepository : IHourRepository
    {
        private VinculacionContext db;
        public HourRepository()
        {
            db = new VinculacionContext();
        }
        public Hour Delete(long id)
        {
            var found = Get(id);
            db.Hours.Remove(found);
            return found;
        }

        public Hour Get(long id)
        {
            return db.Hours.Find(id);
        }

        public IEnumerable<Hour> GetAll()
        {
            return db.Hours;
        }     

        public void Insert(Hour ent)
        {
            db.Hours.Add(ent);
        }

        public Hour InsertHourFromModel(HourEntryModel model)
        {
            var sectionProjectRel = db.SectionProjectsRels.Include(x => x.Project).Include(y => y.Section).FirstOrDefault(z => z.Section.Id == model.SectionId && z.Project.Id == model.ProjectId);
            var user = db.Users.FirstOrDefault(x => x.AccountId == model.AccountId);
            if (user != null && sectionProjectRel != null)
            {
                var hour = new Hour();
                hour.Amount = model.Hour;
                hour.SectionProject = sectionProjectRel;
                hour.User = user;
                Insert(hour);
                return hour;
            }
            return null;
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(Hour ent)
        {
            db.Entry(ent).State = EntityState.Modified;
        }
    }
}