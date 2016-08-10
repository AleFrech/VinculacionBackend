using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Castle.Components.DictionaryAdapter;
using VinculacionBackend.Data.Database;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Data.Models;

namespace VinculacionBackend.Data.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly VinculacionContext _db;
        public ProjectRepository()
        {
            _db = new VinculacionContext();
        }

        public IQueryable<Project> GetByMajor(string majorId)
        {
            var majorProjects = _db.ProjectMajorRels.Include(a => a.Project).Include(b => b.Major).Where(pm => pm.Major.MajorId == majorId);
            List<Project> projects = new List<Project>();
            foreach (var mp in majorProjects)
            {
                projects.Add(mp.Project);
            }
            return projects.AsQueryable();
        }

        public List<MajorProjectTotalmodel> GetMajorProjectTotal(Period currentPeriod, string majorId)
        {
            List<MajorProjectTotalmodel> projectTotal = new EditableList<MajorProjectTotalmodel>();
            var projectsId =
                _db.SectionProjectsRels.Where(x => x.Section.Period.Number == currentPeriod.Number && x.Section.Period.Year == currentPeriod.Year)
                    .Select(x => x.Project.Id)
                    .ToList();
            foreach (var p in projectsId)
            {
                var result =
                    _db.ProjectMajorRels.Where(x => x.Major.MajorId == majorId && x.Project.Id == p)
                        .Where(x => x.Project != null)
                        .Select(x => x.Major.Name).Distinct().ToList();
                if (result.Count > 0)
                {
                    projectTotal.Add(new MajorProjectTotalmodel
                    {
                        Major = result.ElementAt(0),
                        Total = result.Count
                    });
                }
            }
            return projectTotal;
        }

        public IQueryable<Project> GetProjectsByClass(long classId)
        {
            var projects =
                _db.SectionProjectsRels.Include(a => a.Section)
                    .Include(b => b.Project)
                    .Include(c => c.Section.Class)
                    .Where(x => x.Section.Class.Id == classId)
                    .Select(x => x.Project);
            return projects;
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


        public Section GetSection(Project project)
        {
            var rels = _db.SectionProjectsRels.Include(a => a.Section).Include(b => b.Project).FirstOrDefault(x=>x.Project.Id==project.Id);
            if (rels != null)
            {
                var section = _db.Sections.Include(a => a.Class).Include(b => b.Period).Include(c => c.User).FirstOrDefault(x => x.Id == rels.Section.Id);
                return section;
            }

            throw new Exception("not found");

        }


        public Project Get(long id)
        {
            return _db.Projects.FirstOrDefault(x=>x.Id == id && x.IsDeleted == false);
        }

        public IQueryable<Project> GetAll()
        {
            return _db.Projects.Where(x => x.IsDeleted == false);
        }

        public IQueryable<Project> GetAllStudent(long studentId)
        {

            var sectionUserRels = _db.SectionUserRels.Include("Section").Include("User").ToList();
            var projectSectionRels = _db.SectionProjectsRels.Include("Section").Include("Project").ToList();
            return projectSectionRels.Where(rel => sectionUserRels.Any(su => su.Section.Id == rel.Section.Id && su.User.Id == studentId)).Select(rel => rel.Project).Distinct().AsQueryable();
        }

        public IQueryable<Project> GetAllProfessor(long professorId)
        {
            return _db.SectionProjectsRels.Include("Project").Include("Section.User").Where(rel => rel.Section.User.Id == professorId).Select(rel => rel.Project).Distinct();
        }

        public void Insert(Project ent)
        {
            _db.Projects.Add(ent);
        }
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Insert(Project ent, List<string> majorIds, List<long> sectionIds)
        {
            var majors = _db.Majors.Where(x => majorIds.Any(y => y == x.MajorId)).ToList();
            foreach (var major in majors)
            {
                _db.ProjectMajorRels.Add(new ProjectMajor { Project = ent, Major = major });
            }
            var sections = _db.Sections.Where(x=>sectionIds.Any(y=>y==x.Id)).ToList();
            foreach (var section in sections)
            {
                _db.SectionProjectsRels.Add(new SectionProject { Project = ent, Section = section });
            }           

            Insert(ent);
        }

        public void Update(Project ent)
        {
            _db.Entry(ent).State = EntityState.Modified;
        }

        public IQueryable<User> GetProjectStudents(long projectId)
        {
            var sections = _db.SectionProjectsRels.Where(a => a.Project.Id == projectId).Select(b => b.Section).ToList();
            var users = new List<User>();
            foreach (var section in sections)
            {
                users.InsertRange(0, _db.SectionUserRels.Where(a => a.Section.Id == section.Id).Select(b => b.User));
            }
            return users.AsQueryable();
        }

        public void AssignToSection(long projectId, long sectionId)
        {
            var project = Get(projectId);
            var section = _db.Sections.FirstOrDefault(x => x.Id == sectionId);

            _db.SectionProjectsRels.Add(new SectionProject { Project = project, Section = section });

            
        }
        
        public SectionProject RemoveFromSection(long projectId, long sectionId)
        {
            var found = _db.SectionProjectsRels.Include(x=>x.Project).Include(y=>y.Section).FirstOrDefault(z=>z.Project.Id == projectId && z.Section.Id == sectionId);
            if (found!=null)
            {
                _db.SectionProjectsRels.Remove(found);
                Save();
            }
            
            return found;
        }

        public IQueryable<User> GetProfessorsByProject(long projectId)
        {
            return
                _db.SectionProjectsRels.Include(a => a.Section)
                    .Include(b => b.Project)
                    .Include(x => x.Section.User)
                    .Where(x => x.Project.Id == projectId)
                    .Select(x => x.Section.User);
        }

        public Period GetPeriodByProject(long projectId)
        {
            return
                _db.SectionProjectsRels.Include(a => a.Section)
                    .Include(b => b.Project)
                    .Include(x => x.Section.Period)
                    .Where(x => x.Project.Id == projectId)
                    .Select(x => x.Section.Period).First();
        }

        public string GetClass(long sectionId)
        {
            var selectedClass = _db.Sections.Where(a => a.Id == sectionId).Include(b => b.Class).Select(c => c.Class.Name).ToList();
            if (selectedClass.Count < 0)
            {
                return "";
            }
            return selectedClass[0];
        }

        public string GetMajors(List<Major> majors)
        {
            if (majors.Count > 1)
                return "Varias";
            else if (majors.Count < 1)
                return "";
            return majors[0].Name;
        }

        public string GetProfessor(long projectId)
        {
            var selectedClass = _db.SectionProjectsRels.Where(a => a.Project.Id == projectId).ToList();
            if (selectedClass.Count < 0 || selectedClass[0].Section.User == null)
            {
                return "";
            }
            return selectedClass[0].Section.User.Name;
        }

        public string GetTotalHours(long id)
        {
            return (_db.Hours.Where(hours => hours.SectionProject.Project.Id == id).Sum(a => (int?)a.Amount) ?? 0).ToString();
        }

        public IQueryable<Project> GetByYearAndPeriod(int year, int period)
        {
            return _db.SectionProjectsRels.Where(a => a.Section.Period.Number == period && a.Section.Period.Year == year)
                .Select(b => b.Project).Distinct().Where(x => x.IsDeleted == false);
        }

        public IQueryable<Project> GetProjectsBySection(long sectionId)
        {
             return _db.SectionProjectsRels.Include(rel => rel.Section).Include(rel => rel.Project).
                Where(rel => rel.Section.Id == sectionId).Select(rel => rel.Project).Where(x=>x.IsDeleted==false);

        }
    }
}