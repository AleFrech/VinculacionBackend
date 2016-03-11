using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VinculacionBackend.Database;
using VinculacionBackend.Entities;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using WebGrease.Css.Extensions;
using VinculacionBackend.Enums;
using System.Data.Entity;

namespace VinculacionBackend
{
    public class StudentRepository : IStudentRepository
    {
        private VinculacionContext db;

        public StudentRepository()
        {
            db = new VinculacionContext();
        }
        public User Delete(long id)
        {
            var found = Get(id);
            db.Users.Remove(found);
            return found;
        }

        public User DeleteByAccountNumber(string accountNumber)
        {
            var found = GetByAccountNumber(accountNumber);
            if (found != null) {
                var userrole =db.UserRoleRels.FirstOrDefault(x => x.User.AccountId == found.AccountId);
                db.UserRoleRels.Remove(userrole);
                db.Users.Remove(found);
            }
            return found;
        }

        public User Get(long id)
        {
            var rels = GetUserRoleRelationships();
            var student = db.Users.Include(m => m.Major).Where(x => rels.Any(y => y.User.Id == x.Id)).FirstOrDefault(z => z.Id == id);
            return student;
        }

        public IEnumerable<User> GetAll()
        {
            var rels = GetUserRoleRelationships();
            var students = db.Users.Include(m => m.Major).Where(x => rels.Any(y => y.User.Id == x.Id));
            return students;
        }

        public User GetByAccountNumber(string accountNumber)
        {
            var rels = GetUserRoleRelationships();
            var student = db.Users.Include(m => m.Major).Where(x => rels.Any(y => y.User.Id == x.Id)).FirstOrDefault(z => z.AccountId == accountNumber);
            return student;
        }

        public int GetStudentHours(string accountNumber)
        {
            var total = 0;
            var rels = GetUserRoleRelationships();
            var student = db.Users.Include(m => m.Major).Where(x => rels.Any(y => y.User.Id == x.Id)).FirstOrDefault(z => z.AccountId == accountNumber);
            if (student == null)
            {
                return total;
            }
            var hour = db.Hours.Include(a => a.User).Where(x => x.User.Id == student.Id);
            hour.ForEach(x =>
            {
                total += x.Amount;
            });

            return total;
        }

        public IEnumerable<User> GetStudentsByStatus(string status)
        {
            var rels = GetUserRoleRelationships();
            if (status == "Inactive")
                return db.Users.Include(m => m.Major).Where(x => rels.Any(y => y.User.Id == x.Id) && x.Status == Status.Inactive);
            if (status == "Active")
                return db.Users.Include(m => m.Major).Where(x => rels.Any(y => y.User.Id == x.Id) && x.Status == Status.Active);
            if (status == "Verified")
                return db.Users.Include(m => m.Major).Where(x => rels.Any(y => y.User.Id == x.Id) && x.Status == Status.Verified);
            if (status == "Rejected")
                return db.Users.Include(m => m.Major).Where(x => rels.Any(y => y.User.Id == x.Id) && x.Status == Status.Rejected);

            return new List<User>();
        }

        public IEnumerable<User> GetStudentsByStatus(Status status)
        {
            var rels = GetUserRoleRelationships();
            return db.Users.Include(m => m.Major).Where(x => rels.Any(y => y.User.Id == x.Id) && x.Status == Status.Inactive).ToList();
        }

        public void Insert(User ent)
        {
            db.Users.Add(ent);
            db.UserRoleRels.Add(new UserRole { User=ent,Role=db.Roles.FirstOrDefault(x=>x.Name=="Student")});
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(User ent)
        {
            db.Entry(ent).State = EntityState.Modified;
        }

        private IEnumerable<UserRole> GetUserRoleRelationships()
        {
            return db.UserRoleRels.Include(x => x.Role).Include(y => y.User).Where(z => z.Role.Name == "Student");
        }
    }
}