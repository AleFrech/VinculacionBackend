using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using VinculacionBackend.Data.Database;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;

namespace VinculacionBackend.Data.Repositories
{
    public class SectionRepository : ISectionRepository
    {
        private readonly VinculacionContext _db;

        public SectionRepository()
        {
            _db = new VinculacionContext();
        }

        public void AssignStudents(long sectionId, List<string >studentsIds)
        {
            StudentsAreNotInSectionOrClass(sectionId, studentsIds);
            var section = Get(sectionId);

            foreach (var studentId in studentsIds)
            {
                var student = _db.Users.FirstOrDefault(x => x.AccountId == studentId);
                if (student != null)
                {    
                    _db.SectionUserRels.Add(new SectionUser { Section = section, User = student });
                }
            }          
        }
        
        private void StudentsAreNotInSectionOrClass(long sectionId, List<string> studentsIds)
        {
            foreach (var studentId in studentsIds)
            {
                if (!StudentIsNotInSectionOrClass(sectionId, studentId))
                {
                    throw new Exception("El Alumno " + studentId + " ya esta registrado en esta clase");
                }
            }
        }

        private bool StudentIsNotInSectionOrClass(long sectionId, string studentId)
        {
            var section = _db.Sections.Include(x => x.Class).FirstOrDefault(y => y.Id == sectionId);
            var sectionStudent = _db.SectionUserRels.Include(x=>x.Section).Include(y=>y.User).Include(z=>z.Section.Class).FirstOrDefault(a=>a.User.AccountId == studentId && (a.Section.Id == sectionId || a.Section.Class.Id == section.Class.Id));
            if(sectionStudent != null)
            {
                return false;
            }

            return true;
        }

        public void RemoveStudents(long sectionId, List<string> studentsIds)
        {
            foreach (var studentId in studentsIds)
            {
                var student = _db.Users.FirstOrDefault(x => x.AccountId == studentId);
                if (student != null)
                {
                    var found = _db.SectionUserRels.FirstOrDefault(x => x.Section.Id == sectionId && x.User.AccountId == studentId);
                    _db.SectionUserRels.Remove(found);
                }
            }   
        }

        public Section Delete(long id)
        {
            var found = Get(id);
            _db.Sections.Remove(found);
            return found;
        }

        public Section Get(long id)
        {
            return _db.Sections.Include(a => a.Class).Include(b => b.User).Include(c => c.Period).FirstOrDefault(d=>d.Id==id);
        }

        public IQueryable<User> GetSectionStudents(long sectionId)
        {
            var secUserRel = _db.SectionUserRels.Include(a => a.Section).Where(c => c.Section.Id == sectionId);
            var users = _db.Users.Include(a => a.Major).Where(b => secUserRel.Any(c => c.User.Id == b.Id));
            var userRolsRel =_db.UserRoleRels.Include(x => x.Role).Include(y => y.User).Where(z => z.Role.Name == "Student");
            var students = users.Where(x => userRolsRel.Any(y => y.User.Id == x.Id));
            return students;
        }


        public IQueryable<Project> GetSectionProjects(long sectionId)
        {
            var secProjRel = _db.SectionProjectsRels.Include(a => a.Project).Include(b => b.Section).Where(c=>c.Section.Id==sectionId);
            var projects =_db.Projects.Where(x => secProjRel.Any(a => a.Section.Id == x.Id) && x.IsDeleted == false).ToList();

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

        public IQueryable<Section> GetAll()
        {
            return _db.Sections.Include(a => a.Class).Include(b => b.User).Include(c => c.Period);
        }

        public void Insert(Section ent)
        {
            _db.Classes.Attach(ent.Class);
            _db.Periods.Attach(ent.Period);
            _db.Users.Attach(ent.User);
            _db.Sections.Add(ent);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Section ent)
        {
            _db.Entry(ent).State = EntityState.Modified;
        }
    }
}