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

        public List<MajorProjectTotalmodel> GetMajorProjectTotal(int period, int year, string majorId)
        {
            List<MajorProjectTotalmodel> projectTotal = new EditableList<MajorProjectTotalmodel>();
            var projectsId =
                _db.SectionProjectsRels.Where(x => x.Section.Period.Number == period && x.Section.Period.Year == year)
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
            var  project= _db.Projects.FirstOrDefault(x=>x.Id == id && x.IsDeleted == false);
            if (project != null)
            {
                var projectmajors = _db.ProjectMajorRels.Include(a=>a.Project).Include(b=>b.Major).Where(x => x.Project.Id == id).ToList();
                var majorsId = new List<string>();
                foreach (var x in projectmajors)
                {
                    majorsId.Add(x.Major.MajorId);
                }
                project.MajorIds = majorsId;

                var projectSections =
                    _db.SectionProjectsRels.Include(a => a.Project)
                        .Include(b => b.Section)
                        .Where(x => x.Project.Id == id).ToList();

                var sectionsId = new List<long>();
                foreach (var x in projectSections)
                {
                    sectionsId.Add(x.Section.Id);
                }
                project.SectionIds = sectionsId;
            }
           
            return project;
        }

        public IQueryable<Project> GetAll()
        {
            var projects = _db.Projects.Where(x => x.IsDeleted == false).ToList();
            foreach (var project in projects)
            {
                var projectmajors =_db.ProjectMajorRels.Include(a => a.Project).Include(b => b.Major).Where(x => x.Project.Id == project.Id).ToList();
                var majorsId = new List<string>();
                foreach (var x in projectmajors)
                {
                    majorsId.Add(x.Major.MajorId);
                }
                project.MajorIds = majorsId;
                var projectSections =_db.SectionProjectsRels.Include(a => a.Project) .Include(b => b.Section) .Where(x => x.Project.Id ==project.Id).ToList();
                var sectionsId = new List<long>();
                foreach (var x in projectSections)
                {
                    sectionsId.Add(x.Section.Id);
                }
                project.SectionIds = sectionsId;
            }
            return  projects.AsQueryable();
        }

        public IQueryable<Project> GetAllStudent(long studentId)
        {
            var allProjects = _db.Projects.Where(x => x.IsDeleted == false).ToList();
            var projects = new List<Project>();
            foreach (var project in allProjects)
            {
                var isValid = false;
                var projectmajors = _db.ProjectMajorRels.Include(a => a.Project).Include(b => b.Major).Where(x => x.Project.Id == project.Id).ToList();
                var majorsId = new List<string>();
                foreach (var x in projectmajors)
                {
                    majorsId.Add(x.Major.MajorId);
                }
                project.MajorIds = majorsId;
                var projectSections = _db.SectionProjectsRels.Include(a => a.Project).Include(b => b.Section).Where(x => x.Project.Id == project.Id).Include(s => s.Section.User).ToList();
                var sectionsId = new List<long>();
                foreach (var x in projectSections)
                {
                    sectionsId.Add(x.Section.Id);
                    var section = x;
                    var students = _db.SectionUserRels.Where(a => a.Section.Id == section.Section.Id)
                                                      .Where(a => a.User.Id == studentId);
                    isValid = students.Any();
                }
                project.SectionIds = sectionsId;
                if (isValid)
                {
                    projects.Add(project);
                }
            }
            return projects.AsQueryable();
        }

        public IQueryable<Project> GetAllProfessor(long professorId)
        {
            var allProjects = _db.Projects.Where(x => x.IsDeleted == false).ToList();
            var projects = new List<Project>();
            foreach (var project in allProjects)
            {
                var isValid = false;
                var projectmajors = _db.ProjectMajorRels.Include(a => a.Project).Include(b => b.Major).Where(x => x.Project.Id == project.Id).ToList();
                var majorsId = new List<string>();
                foreach (var x in projectmajors)
                {
                    majorsId.Add(x.Major.MajorId);
                }
                project.MajorIds = majorsId;
                var projectSections = _db.SectionProjectsRels.Include(a => a.Project).Include(b => b.Section).Where(x => x.Project.Id == project.Id).Include(s => s.Section.User).ToList();
                var sectionsId = new List<long>();
                foreach (var x in projectSections)
                {
                    sectionsId.Add(x.Section.Id);
                    isValid = x.Section.User != null && (x.Section.User.Id == professorId || isValid);
                }
                project.SectionIds = sectionsId;
                if (isValid)
                {
                    projects.Add(project);
                }
            }
            return projects.AsQueryable();
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

            var projectMajors =
                _db.ProjectMajorRels.Include(a => a.Project)
                    .Include(b => b.Major)
                    .Where(c => c.Project.Id == ent.Id)
                    .ToList();

            foreach (var pm in projectMajors)
            {
                _db.ProjectMajorRels.Remove(pm);
            }
              
            var majors = _db.Majors.Where(x => ent.MajorIds.Any(y => y == x.MajorId)).ToList();
            foreach (var major in majors)
            {
               _db.ProjectMajorRels.Add(new ProjectMajor { Project = ent, Major = major });
            }

            var projecSections =
               _db.SectionProjectsRels.Include(a => a.Project)
                   .Include(b => b.Section)
                   .Where(c => c.Project.Id == ent.Id)
                   .ToList();
            foreach (var ps in projecSections)
            {
                _db.SectionProjectsRels.Remove(ps);
            }
            var sections = _db.Sections.Where(x => ent.SectionIds.Any(y => y == x.Id)).ToList();
            foreach (var s in sections)
            {
                _db.SectionProjectsRels.Add(new SectionProject { Project = ent, Section = s });
            }
            _db.Entry(ent).State = EntityState.Modified;
        }

        public IQueryable<User> GetProjectStudents(long projectId)
        {
            var secProjRel = _db.SectionProjectsRels.Include(a => a.Project).Where(c => c.Project.Id == projectId);
            var horas = _db.Hours.Include(a => a.SectionProject).Include(b => b.User).Where(c => secProjRel.Any(d => d.Id == c.SectionProject.Id));
            var users = _db.Users.Include(a => a.Major).Where(b => horas.Any(c => c.User.Id == b.Id));
            return users;
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

        public string getClass(long sectionId)
        {
            var selectedClass = _db.Sections.Where(a => a.Id == sectionId).Include(b => b.Class).Select(c => c.Class.Name).ToList();
            if (selectedClass.Count < 0)
            {
                return "";
            }
            return selectedClass[0];
        }

        public string getMajors(List<string> majorIds)
        {
            if (majorIds.Count > 1)
                return "Varias";
            else if (majorIds.Count < 1)
                return "";
            string majorId = majorIds[0];
            return _db.Majors.First(a => a.MajorId == majorId).Name;
        }

        public string getProfessor(long projectId)
        {
            var selectedClass = _db.SectionProjectsRels.Where(a => a.Project.Id == projectId).ToList();
            if (selectedClass.Count < 0 || selectedClass[0].Section.User == null)
            {
                return "";
            }
            return selectedClass[0].Section.User.Name;
        }

        public string getTotalHours(long id)
        {
            return (_db.Hours.Where(hours => hours.SectionProject.Project.Id == id).Sum(a => (int?)a.Amount) ?? 0).ToString();
        }

        public IQueryable<Project> GetByYearAndPeriod(int year, int period)
        {
            var projects = _db.SectionProjectsRels.Where(a => a.Section.Period.Number == period && a.Section.Period.Year == year)
                .Select(b => b.Project).Distinct().Where(x => x.IsDeleted == false).ToList();
            foreach (var project in projects)
            {
                var projectmajors = _db.ProjectMajorRels.Include(a => a.Project).Include(b => b.Major).Where(x => x.Project.Id == project.Id).ToList();
                var majorsId = new List<string>();
                foreach (var x in projectmajors)
                {
                    majorsId.Add(x.Major.MajorId);
                }
                project.MajorIds = majorsId;
                var projectSections = _db.SectionProjectsRels.Include(a => a.Project).Include(b => b.Section).Where(x => x.Project.Id == project.Id).ToList();
                var sectionsId = new List<long>();
                foreach (var x in projectSections)
                {
                    sectionsId.Add(x.Section.Id);
                }
                project.SectionIds = sectionsId;
            }
            return projects.AsQueryable();
        }
    }
}