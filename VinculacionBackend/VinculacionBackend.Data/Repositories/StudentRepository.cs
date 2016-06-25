using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using VinculacionBackend.Data.Database;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Enums;
using VinculacionBackend.Data.Interfaces;

namespace VinculacionBackend.Data.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly VinculacionContext _db;
        public StudentRepository()
        {
            _db = new VinculacionContext();
        }
        public User Delete(long id)
        {
            var found = Get(id);
            _db.Users.Remove(found);
            return found;
        }

        public  Dictionary<User, int> GetStudentsHoursByProject(long projectId)
        {
            var secProjRel = _db.SectionProjectsRels.Include(a => a.Project).Where(c => c.Project.Id == projectId);
            var horas = _db.Hours.Include(a => a.SectionProject).Include(b => b.User).Where(c => secProjRel.Any(d => d.Id == c.SectionProject.Id));
            var projectStudents = _db.Users.Include(a => a.Major).Include(f => f.Major.Faculty).Where(b => horas.Any(c => c.User.Id == b.Id)).ToList();
            Dictionary<User, int> studentHours = new Dictionary<User, int>();
            foreach (var projectStudent in projectStudents)
            {
                var hours = GetStudentHoursByProject(projectStudent.AccountId, projectId);
                studentHours[projectStudent] = hours;
            }
            return studentHours;
        }

        public User DeleteByAccountNumber(string accountNumber)
        {
            var found = GetByAccountNumber(accountNumber);
            if (found != null) {
                var userrole =_db.UserRoleRels.FirstOrDefault(x => x.User.AccountId == found.AccountId);
                _db.UserRoleRels.Remove(userrole);
                _db.Users.Remove(found);
            }
            return found;
        }

        public string GetStudentMajors(List<User> students)
        {
            List<string> majors = new List<string>();

            foreach (var student in students)
            {
                if (!majors.Contains(student.Major.Name))
                {
                    majors.Add(student.Major.Name);
                }
            }

            string texts = "";

            for (int i = 0; i < majors.Count; i++)
            {
                if (i > 0)
                {
                    texts += " / ";
                }
                texts += majors[i];
            } 
        

            return texts;
        }

        public User Get(long id)
        {
            var rels = GetUserRoleRelationships();
            var student = _db.Users.Include(m => m.Major).Include(f=>f.Major.Faculty).Where(x => rels.Any(y => y.User.Id == x.Id)).FirstOrDefault(z => z.Id == id);
            return student;
        }

        public IQueryable<User> GetAll()
        {
            var rels = GetUserRoleRelationships();
            var students = _db.Users.Include(m => m.Major).Include(f => f.Major.Faculty).Where(x => rels.Any(y => y.User.Id == x.Id));
            return students;
        }

        public User GetByAccountNumber(string accountNumber)
        {
            var rels = GetUserRoleRelationships();
            var student = _db.Users.Include(m => m.Major).Include(f => f.Major.Faculty).Where(x => rels.Any(y => y.User.Id == x.Id)).FirstOrDefault(z => z.AccountId == accountNumber);
            return student;
        }

        public int GetStudentHours(string accountNumber)
        {
            var total = 0;
            var rels = GetUserRoleRelationships();
            var student = _db.Users.Include(m => m.Major).Include(f => f.Major.Faculty).Where(x => rels.Any(y => y.User.Id == x.Id)).FirstOrDefault(z => z.AccountId == accountNumber);
            if (student == null)
            {
                return total;
            }
            var hour = _db.Hours.Include(a => a.User).Where(x => x.User.Id == student.Id);

            foreach (var x in hour)
            {
                total += x.Amount;
            }
            return total;
        }

        public IEnumerable<User> GetStudentsByStatus(string status)
        {
            var rels = GetUserRoleRelationships();
            if (status == "Inactive")
                return _db.Users.Include(m => m.Major).Include(f => f.Major.Faculty).Where(x => rels.Any(y => y.User.Id == x.Id) && x.Status == Status.Inactive);
            if (status == "Active")
                return _db.Users.Include(m => m.Major).Include(f => f.Major.Faculty).Where(x => rels.Any(y => y.User.Id == x.Id) && x.Status == Status.Active);
            if (status == "Verified")
                return _db.Users.Include(m => m.Major).Include(f => f.Major.Faculty).Where(x => rels.Any(y => y.User.Id == x.Id) && x.Status == Status.Verified);
            if (status == "Rejected")
                return _db.Users.Include(m => m.Major).Include(f => f.Major.Faculty).Where(x => rels.Any(y => y.User.Id == x.Id) && x.Status == Status.Rejected);

            return new List<User>();
        }

        public User GetByEmail(string email)
        {
            var rels = GetUserRoleRelationships();
            var student = _db.Users.Include(m => m.Major).Include(f => f.Major.Faculty).Where(x => rels.Any(y => y.User.Id == x.Id)).FirstOrDefault(z => z.Email == email);
            return student;
        }

        public IEnumerable<User> GetStudentsByStatus(Status status)
        {
            var rels = GetUserRoleRelationships();
            return _db.Users.Include(m => m.Major).Include(f => f.Major.Faculty).Where(x => rels.Any(y => y.User.Id == x.Id) && x.Status == Status.Inactive).ToList();
        }

        public void Insert(User ent)
        { 

            _db.Majors.Attach(ent.Major);
            _db.Users.Add(ent);
            _db.UserRoleRels.Add(new UserRole { User=ent,Role=_db.Roles.FirstOrDefault(x=>x.Name=="Student")});
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(User ent)
        {
            _db.Majors.Attach(ent.Major);
            _db.Users.AddOrUpdate(ent);
            
        }

        private IEnumerable<UserRole> GetUserRoleRelationships()
        {
            return _db.UserRoleRels.Include(x => x.Role).Include(y => y.User).Where(z => z.Role.Name == "Student");
        }

        public int GetStudentHoursByProject(string accountNumber, long projectId)
        {
            var student = GetByAccountNumber(accountNumber);
            if(student == null)
            {
                throw new Exception("Student Not Found");
            }
            var studentHours = _db.Hours.Include(a => a.User).Include(b=>b.SectionProject).Where(x => x.User.Id == student.Id).ToList();

            var total = 0;
            foreach(var studentHour in studentHours)
            {
                var sectionProject = _db.SectionProjectsRels.Include(a => a.Project).FirstOrDefault(x => x.Id == studentHour.Id);
                if(sectionProject != null && sectionProject.Project != null & sectionProject.Project.Id == projectId)
                {
                    total += studentHour.Amount;
                }
                
            }

            return total;
        }

    }
}