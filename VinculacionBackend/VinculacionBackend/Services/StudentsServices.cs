using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VinculacionBackend.Database;
using VinculacionBackend.Entities;
using VinculacionBackend.Enums;
using VinculacionBackend.Models;

namespace VinculacionBackend.Services
{
    public class StudentsServices : IUsersServices
    {
        private VinculacionContext db = new VinculacionContext();
        public  User Map(User user, UserEntryModel userModel)
        {
            var newUser = new User();
            newUser.AccountId = userModel.AccountId;
            newUser.Name = userModel.Name;
            newUser.Password = EncryptDecrypt.Encrypt(userModel.Password);
            newUser.Major = db.Majors.FirstOrDefault(x => x.MajorId == userModel.MajorId);
            newUser.Campus = userModel.Campus;
            newUser.Email = userModel.Email;
            newUser.Status = Status.Inactive;
            newUser.CreationDate = DateTime.Now;
            newUser.ModificationDate = DateTime.Now;
            return newUser;
        }

        public void Add(User user)
        {
            db.Users.Add(user);
            db.UserRoleRels.Add(new UserRole { User = user, Role = db.Roles.FirstOrDefault(x => x.Name == "Student") });
            db.SaveChanges();
            
        }

        public User Find(string accountId)
        {
            var rels = db.UserRoleRels.Include(x => x.Role).Include(y => y.User).Where(z => z.Role.Name == "Student");
            var student = db.Users.Include(m => m.Major).Where(x => rels.Any(y => y.User.Id == x.Id)).FirstOrDefault(z => z.AccountId == accountId);
            return student;
        }

        public IQueryable<User> ListbyStatus(string status)
        {
            var rels = db.UserRoleRels.Include(x => x.Role).Include(y => y.User).Where(z => z.Role.Name == "Student");
       
            if (status == "Inactive")
                return db.Users.Include(m => m.Major).Where(x => rels.Any(y => y.User.Id == x.Id) && x.Status == Status.Inactive);
            if (status == "Active")
                return db.Users.Include(m => m.Major).Where(x => rels.Any(y => y.User.Id == x.Id) && x.Status == Status.Active);
            if (status == "Verified")
                return db.Users.Include(m => m.Major).Where(x => rels.Any(y => y.User.Id == x.Id) && x.Status == Status.Verified);
            if (status == "Rejected")
                return db.Users.Include(m => m.Major).Where(x => rels.Any(y => y.User.Id == x.Id) && x.Status == Status.Rejected);
            return null;
        }

        public User RejectUser(string accountId, string message)
        {

            var student = Find(accountId);
            if (student != null)
            {
                MailManager.SendSimpleMessage(student.Email, message, "Vinculación");
                student.Status = Status.Rejected;
                db.SaveChanges();
           }
            return student;
        }

        public bool AcceptUser(string accountId)
        {
            throw new NotImplementedException();
        }

        public User DeleteUser(string accountId)
        {
            throw new NotImplementedException();
        }
    }
}