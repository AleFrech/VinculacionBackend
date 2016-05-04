using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using VinculacionBackend.Data.Database;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;

namespace VinculacionBackend.Data.Repositories
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
            var majorRels = db.ProjectMajorRels.Where(x => x.Project.Id == found.Id);
            foreach (var rel in majorRels)
            {
                db.ProjectMajorRels.Remove(rel);
            }
            var sectionRels = db.SectionProjectsRels.Where(x => x.Project.Id == found.Id);
            foreach (var r in sectionRels)
            {
                db.SectionProjectsRels.Remove(r);
            }
            db.Projects.Remove(found);

            return found;
        }

        public Project Get(long id)
        {
            return db.Projects.Find(id);
        }

        public IQueryable<Project> GetAll()
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

        public IQueryable<User> GetProjectStudents(long projectId)
        {
            var secProjRel = db.SectionProjectsRels.Include(a => a.Project).Where(c => c.Project.Id == projectId);
            var horas = db.Hours.Include(a => a.SectionProject).Include(b => b.User).Where(c => secProjRel.Any(d => d.Id == c.SectionProject.Id));
            var users = db.Users.Include(a => a.Major).Where(b => horas.Any(c => c.User.Id == b.Id));
            return users;
        }

        public void Insert(Project ent, List<string> majorIds)
        {
            var majors = db.Majors.Where(x => majorIds.Any(y => y == x.MajorId));
            foreach (var major in majors)
            {
                db.ProjectMajorRels.Add(new ProjectMajor { Project = ent, Major = major });
            }

            Insert(ent);
        }

        public void AssignToSection(Project ent, long sectionId)
        {
            var project = Get(ent.Id);
            var section = db.Sections.FirstOrDefault(x => x.Id == sectionId);

            if(project != null && section != null)
            {
                db.SectionProjectsRels.Add(new SectionProject { Project = project, Section = section });
            }
        }
    }
}