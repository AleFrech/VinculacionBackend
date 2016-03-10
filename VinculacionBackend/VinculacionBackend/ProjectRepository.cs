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

        public IEnumerable<User> GetProjectParticipants(long id)
        {
            var secProjRel = db.SectionProjectsRels.Include(a => a.Project).Where(c => c.Project.Id == id);
            var horas = db.Hours.Include(a => a.SectionProject).Include(b => b.User).Where(c => secProjRel.Any(d => d.Id == c.SectionProject.Id));
            var users = db.Users.Include(a => a.Major).Where(b => horas.Any(c => c.User.Id == b.Id));
            return users;
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