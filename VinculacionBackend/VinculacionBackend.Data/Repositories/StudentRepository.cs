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

        public  Dictionary<User, int> GetStudentsHoursByProject(long sectionId, long projectId)
        {
            var secProjRel = _db.SectionProjectsRels.Include(a => a.Project).Include(b=>b.Section).Where(c => c.Project.Id == projectId &&  c.Section.Id==sectionId);
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
            var studentHours = _db.Hours.Include(a => a.User).Include(b => b.SectionProject).Include(c => c.SectionProject.Project).Where(x => x.User.Id == student.Id).ToList();

            var total = 0;
            foreach (var studentHour in studentHours)
            {
                var sectionProject = _db.SectionProjectsRels.Include(a => a.Project).FirstOrDefault(x => x.Id == studentHour.SectionProject.Id);
                if (sectionProject == null)
                    continue;
                if(sectionProject.Project.Id == projectId)

                {
                    total += studentHour.Amount;
                }

            }

            return total;
        }

        public StudentCountModel GetStudentCount( string clase, int year)
        {
            var students = _db.SectionUserRels.Where(a => a.Section.Period.Year == year)
                    .Where(b => b.Section.Class.Name.StartsWith(clase)).Include(d => d.Section).GroupBy(c => c.Section.Period.Number);
            var first = students.Where(f => f.Key == 1).SingleOrDefault();
            var second = students.Where(f => f.Key == 2).SingleOrDefault();
            var fourth = students.Where(f => f.Key == 4).SingleOrDefault();
            var fifth = students.Where(f => f.Key == 5).SingleOrDefault();
            return new StudentCountModel
            {
                
                FirstPeriod = first != null ? first.Count() : 0,
                SecondPeriod = second != null ? second.Count() : 0,
                FourthPeriod = fourth != null ? fourth.Count() : 0,
                FifthPeriod = fifth != null ? fifth.Count() : 0
            };
        }

        public StudentCountModel GetStudentByFacultyCount(int faculty, int year)
        {
            var students = _db.SectionUserRels.Where(a => a.Section.Period.Year == year).Where(a => a.User.Major.Faculty.Id == faculty).GroupBy(c => c.Section.Period.Number);
            var first = students.Where(f => f.Key == 1).SingleOrDefault();
            var second = students.Where(f => f.Key == 2).SingleOrDefault();
            var fourth = students.Where(f => f.Key == 4).SingleOrDefault();
            var fifth = students.Where(f => f.Key == 5).SingleOrDefault();
            return new StudentCountModel
            {

                FirstPeriod = first != null ? first.Count() : 0,
                SecondPeriod = second != null ? second.Count() : 0,
                FourthPeriod = fourth != null ? fourth.Count() : 0,
                FifthPeriod = fifth != null ? fifth.Count() : 0
            };
        }

        public StudentCountModel GetHoursCount( string clase, int year)
        {
            var periodHours = _db.Hours.Where(a => a.SectionProject.Section.Period.Year == year).Where(b => b.SectionProject.Section.Class.Name.StartsWith(clase))
                .GroupBy(c => c.SectionProject.Section.Period.Number);
            var first = periodHours.Where(f => f.Key == 1).SingleOrDefault();
            var second = periodHours.Where(f => f.Key == 2).SingleOrDefault();
            var fourth = periodHours.Where(f => f.Key == 4).SingleOrDefault();
            var fifth = periodHours.Where(f => f.Key == 5).SingleOrDefault();
            return new StudentCountModel
        {

                FirstPeriod = first != null ? first.Sum(a => a.Amount) : 0,
                SecondPeriod = second != null ? second.Sum(a => a.Amount) : 0,
                FourthPeriod = fourth != null ? fourth.Sum(a => a.Amount) : 0,
                FifthPeriod = fifth != null ? fifth.Sum(a => a.Amount) : 0
            };
        }

        public StudentCountModel GetHoursByFacultyCount( int faculty, int year)
        {
            var periodHours = _db.Hours.Where(a =>  a.SectionProject.Section.Period.Year == year).Where(b => b.User.Major.Faculty.Id == faculty)
                .GroupBy(c => c.SectionProject.Section.Period.Number);
            var first = periodHours.Where(f => f.Key == 1).SingleOrDefault();
            var second = periodHours.Where(f => f.Key == 2).SingleOrDefault();
            var fourth = periodHours.Where(f => f.Key == 4).SingleOrDefault();
            var fifth = periodHours.Where(f => f.Key == 5).SingleOrDefault();
            return new StudentCountModel
        {

                FirstPeriod = first != null ? first.Sum(a => a.Amount) : 0,
                SecondPeriod = second != null ? second.Sum(a => a.Amount) : 0,
                FourthPeriod = fourth != null ? fourth.Sum(a => a.Amount) : 0,
                FifthPeriod = fifth != null ? fifth.Sum(a => a.Amount) : 0
            };
        }

        public IEnumerable<User> GetStudentByMajor(string majorId)
        {
            var students = GetAll();
            return students.AsQueryable().Include(x => x.Major).Where(a => a.Major.MajorId == majorId).ToList();
        }

        public int GetStudentHoursBySection(string accountId, long sectionId)
        {
            var user = _db.Hours.Where(a => a.SectionProject.Section.Id == sectionId)
                            .Where(b => accountId.Equals(b.User.AccountId))
                            .FirstOrDefault();
            return user != null ? user.Amount : 0;
        }

        public IQueryable<object> GetStudentSections(string accountId)
        {
            var hours = _db.Hours.Where(a => accountId.Equals(a.User.AccountId)).Include(c=> c.User).Include(b => b.SectionProject.Section).ToList();
            var sections = _db.SectionUserRels.Where(a => a.User.AccountId == accountId)
                                      .Select(b => b.Section).Include(c => c.Class)
                                      .ToList();
            var results = sections.Select(a => new
            {
                Id = a.Id,
                Code = a.Code,
                Class = a.Class,
                HoursWorked = hours.Where(b => b.SectionProject.Section.Id == a.Id)
                                   .DefaultIfEmpty(new Hour { Amount = 0 })
                                   .First().Amount
            });
            return results.AsQueryable();
        }

        public void InsertMany(IList<User> students)
        {
            foreach (var student in students)
            {
                Insert(student);
            }
        }
    }

    public class StudentCountModel
    {
        public int FifthPeriod { get; set; }
        public int FirstPeriod { get; set; }
        public int FourthPeriod { get; set; }
        public int SecondPeriod { get; set; }
    }
}
