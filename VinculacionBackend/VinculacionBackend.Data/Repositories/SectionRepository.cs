using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using VinculacionBackend.Data.Database;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Exceptions;
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

        public void AssignStudents(long sectionId, List<string> studentsIds)
        {
            StudentsAreNotInSectionOrClass(sectionId, studentsIds);
            var section = Get(sectionId);

            foreach (var studentId in studentsIds)
            {
                var student = _db.Users.FirstOrDefault(x => x.AccountId == studentId);
                if (student != null)
                {
                    _db.SectionUserRels.Add(new SectionUser {Section = section, User = student});
                }
            }
        }

        private void StudentsAreNotInSectionOrClass(long sectionId, List<string> studentsIds)
        {
            foreach (var studentId in studentsIds)
            {
                if (!StudentIsNotInOrClass(sectionId, studentId))
                {
                    throw new NotFoundException("El Alumno " + studentId+" ya esta registrado en esta clase en este periodo");
                }
            }
        }

        private bool StudentIsNotInOrClass(long sectionId, string studentId)
        {
            var section =
                _db.Sections.Include(x => x.Class).Include(a => a.Period).FirstOrDefault(y => y.Id == sectionId);
            var sectionStudent = _db.SectionUserRels.Include(x => x.Section).Include(y => y.User)
                .Include(z => z.Section.Class).Include(s => s.Section.Period)
                .FirstOrDefault(
                    a =>
                        a.User.AccountId == studentId && a.Section.Class.Id == section.Class.Id &&
                        section.Period.IsCurrent);
            if (sectionStudent != null)
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
                    var found =
                        _db.SectionUserRels.FirstOrDefault(
                            x => x.Section.Id == sectionId && x.User.AccountId == studentId);
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
            return
                _db.Sections.Include(a => a.Class)
                    .Include(b => b.User)
                    .Include(c => c.Period)
                    .FirstOrDefault(d => d.Id == id);
        }

        public IQueryable<User> GetSectionStudents(long sectionId)
        {
            var secUserRel = _db.SectionUserRels.Include(a => a.Section).Where(c => c.Section.Id == sectionId);
            var users = _db.Users.Include(a => a.Major).Where(b => secUserRel.Any(c => c.User.Id == b.Id));
            var userRolsRel =
                _db.UserRoleRels.Include(x => x.Role).Include(y => y.User).Where(z => z.Role.Name == "Student");
            var students = users.Where(x => userRolsRel.Any(y => y.User.Id == x.Id));
            return students;
        }

        public object GetSectionStudentsHours(long sectionId, long projectId)
        {
            return new
            {
                IsApproved = _db.SectionProjectsRels.Where(a => a.Project.Id == projectId && a.Section.Id == sectionId).Single().IsApproved,
                Hours = _db.SectionProjectsRels.Where(a => a.Project.Id == projectId && a.Section.Id == sectionId)
                    .Join(_db.SectionUserRels, su => su.Section.Id, sp => sp.Section.Id, (su, sp) => new { su, sp })
                    .Select(x => new
                    {
                        User = x.sp.User,
                        Hours = _db.Hours.Where(d => d.User.Id == x.sp.User.Id && d.SectionProject.Id == x.su.Id).FirstOrDefault()
                    })
            };
        }

        public void ClearSectionStudents(long sectionId)
        {
            List<SectionUser> sectionStudents = _db.SectionUserRels.Include(x => x.Section).Where(x => x.Section.Id == sectionId).ToList();
            foreach(var rel in sectionStudents)
            {
                _db.SectionUserRels.Remove(rel);
            }
        }

        public IQueryable<Project> GetSectionProjects(long sectionId)
        {
            var secProjRel =
                _db.SectionProjectsRels.Include(a => a.Project)
                    .Include(b => b.Section)
                    .Where(c => c.Section.Id == sectionId);
            var projects =
                _db.Projects.Where(x => secProjRel.Any(a => a.Project.Id == x.Id) && x.IsDeleted == false).ToList();

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
            _db.Classes.Attach(ent.Class);
            _db.Users.Attach(ent.User);
            _db.Sections.AddOrUpdate(ent);
        }

        public IQueryable<Section> GetAllByStudent(long userId)
        {
            return _db.SectionUserRels.Where(b => b.User.Id == userId).Select(a => a.Section)
                .Include(b => b.Class).Include(c => c.Period).Include(d => d.User);
        }

        public IQueryable<Section> GetSectionsByProject(long projectId)
        {
            return _db.SectionProjectsRels.Include(rel => rel.Section).Include(rel => rel.Project).
                Where(rel => rel.Project.Id == projectId).Select(rel => rel.Section).Include(x=>x.Period).Include(a=>a.User).Include(c=>c.Class);
        }

        public IQueryable<Section> GetAllByProfessor(long userId)
        {
            return _db.Sections.Where(b => b.User.Id == userId)
                .Include(b => b.Class).Include(c => c.Period).Include(d => d.User);
        }

        public IQueryable<SectionUser> GetSectionsUsersRels()
        {
            return _db.SectionUserRels.Include(x => x.Section).Include(x => x.User);
        }
    }
}

