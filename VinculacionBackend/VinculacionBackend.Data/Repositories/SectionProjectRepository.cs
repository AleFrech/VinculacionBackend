using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using VinculacionBackend.Data.Database;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;

namespace VinculacionBackend.Data.Repositories
{
    public class SectionProjectRepository : ISectionProjectRepository
    {
        private readonly VinculacionContext _db;

        public SectionProjectRepository()
        {
            _db = new VinculacionContext();
        }

        public IQueryable<SectionProject> GetAll()
        {
            return _db.SectionProjectsRels.Include(rel => rel.Section).Include(rel => rel.Project);
        }

        public SectionProject Get(long id)
        {
            return
                _db.SectionProjectsRels.Include(rel => rel.Section)
                    .Include(rel => rel.Project)
                    .Include(rel => rel.Section.User)
                    .Include(rel => rel.Section.Class)
                    .FirstOrDefault(rel => rel.Id == id);
        }

        public SectionProject Delete(long id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(SectionProject ent)
        {
            _db.Entry(ent).State = EntityState.Modified;
        }

        public void Insert(SectionProject ent)
        {
            _db.SectionProjectsRels.Add(ent);
        }
    }
}
