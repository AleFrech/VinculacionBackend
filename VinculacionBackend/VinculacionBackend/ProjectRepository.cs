using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VinculacionBackend.Database;
using VinculacionBackend.Entities;
using System.Data.Entity;

namespace VinculacionBackend
{
    public class ProjectRepository : IProjectRepository
    {
        private VinculacionContext db;
        public ProjectRepository()
        {
            db = new VinculacionContext();
        }

        public void Delete(long id)
        {
            var found = Get(id);
            db.Projects.Remove(found);
        }

        public Project Get(long id)
        {
            return db.Projects.Find(id);
        }

        public IEnumerable<Project> GetAll()
        {
            return db.Projects;
        }

        public void Insert(Project ent)
        {
            db.Projects.Add(ent);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(Project ent)
        {
            db.Entry(ent).State = EntityState.Modified;
        }
    }
}