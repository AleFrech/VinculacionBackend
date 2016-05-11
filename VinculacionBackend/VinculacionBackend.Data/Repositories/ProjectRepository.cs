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
        private readonly VinculacionContext _db;
        public ProjectRepository()
        {
            _db = new VinculacionContext();
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
            var  project= _db.Projects.FirstOrDefault(x=>x.Id == id && x.IsDeleted == false);
            if (project != null)
            {
                var projectmajors = _db.ProjectMajorRels.Include(a=>a.Project).Include(b=>b.Major).Where(x => x.Project.Id == id);
                var majorsId = new List<string>();
                foreach (var x in projectmajors)
                {
                    majorsId.Add(x.Major.MajorId);
                }
                project.MajorIds = majorsId;
            }
           
            return project;
        }

        public IQueryable<Project> GetAll()
        {
            var projects = _db.Projects.Where(x => x.IsDeleted == false).ToList();
            foreach (var project in projects)
            {
                var projectmajors =_db.ProjectMajorRels.Include(a => a.Project).Include(b => b.Major).Where(x => x.Project.Id == project.Id);
                var majorsId = new List<string>();
                foreach (var x in projectmajors)
                {
                    majorsId.Add(x.Major.MajorId);
                }
                project.MajorIds = majorsId;
            }
            return  projects.AsQueryable();
        }

        public void Insert(Project ent)
        {
            _db.Projects.Add(ent);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Project ent)
        {
            _db.Entry(ent).State = EntityState.Modified;
        }

        public IQueryable<User> GetProjectStudents(long projectId)
        {
            var secProjRel = _db.SectionProjectsRels.Include(a => a.Project).Where(c => c.Project.Id == projectId);
            var horas = _db.Hours.Include(a => a.SectionProject).Include(b => b.User).Where(c => secProjRel.Any(d => d.Id == c.SectionProject.Id));
            var users = _db.Users.Include(a => a.Major).Where(b => horas.Any(c => c.User.Id == b.Id));
            return users;
        }

        public void Insert(Project ent, List<string> majorIds, long sectionId)
        {
            var majors = _db.Majors.Where(x => majorIds.Any(y => y == x.MajorId));
            foreach (var major in majors)
            {
                _db.ProjectMajorRels.Add(new ProjectMajor { Project = ent, Major = major });
            }
            var section = _db.Sections.FirstOrDefault(x => x.Id == sectionId);

            _db.SectionProjectsRels.Add(new SectionProject { Project = ent, Section = section });

            Insert(ent);
        }

        public void AssignToSection(long projectId, long sectionId)
        {
            var project = Get(projectId);
            var section = _db.Sections.FirstOrDefault(x => x.Id == sectionId);

            _db.SectionProjectsRels.Add(new SectionProject { Project = project, Section = section });

            
        }
    }
}