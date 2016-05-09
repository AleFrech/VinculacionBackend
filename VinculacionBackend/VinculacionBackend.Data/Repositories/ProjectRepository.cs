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

            if(found != null){
                found.IsDeleted = true;
                Save();
            }
            
            return found;
        }

        public Project Get(long id)
        {
            return db.Projects.FirstOrDefault(x=>x.Id == id && x.IsDeleted == false);
        }

        public IQueryable<Project> GetAll()
        {
            return db.Projects.Where(x=>x.IsDeleted == false);
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

        public void AssignToSection(long projectId, long sectionId)
        {
            var project = Get(projectId);
            var section = db.Sections.FirstOrDefault(x => x.Id == sectionId);

            db.SectionProjectsRels.Add(new SectionProject { Project = project, Section = section });

            
        }
    }
}