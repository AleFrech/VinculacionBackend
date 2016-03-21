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

        public IEnumerable<User> GetProjectStudents(long projectId)
        {
            var secProjRel = db.SectionProjectsRels.Include(a => a.Project).Where(c => c.Project.Id == projectId);
            var horas = db.Hours.Include(a => a.SectionProject).Include(b => b.User).Where(c => secProjRel.Any(d => d.Id == c.SectionProject.Id));
            var users = db.Users.Include(a => a.Major).Where(b => horas.Any(c => c.User.Id == b.Id));
            return users;
        }
    }
}