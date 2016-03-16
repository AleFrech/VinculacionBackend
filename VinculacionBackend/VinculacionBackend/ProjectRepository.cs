using System.Collections.Generic;
using System.Data.Entity;
using VinculacionBackend.Database;
using VinculacionBackend.Entities;

namespace VinculacionBackend.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private VinculacionContext db;
        public ProjectRepository()
        {
            db = new VinculacionContext();
        }

        public Project Delete(long id)
        {
            var found = Get(id);
            db.Projects.Remove(found);
            return found;
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