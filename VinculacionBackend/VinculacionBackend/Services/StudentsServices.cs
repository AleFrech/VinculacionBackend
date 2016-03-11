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
        private readonly StudentRepository _studentRepository = new StudentRepository();
        private readonly MajorRepository _majorRepository = new
        public  User Map(User user, UserEntryModel userModel)
        {
            var newUser = new User();
            newUser.AccountId = userModel.AccountId;
            newUser.Name = userModel.Name;
            newUser.Password = EncryptDecrypt.Encrypt(userModel.Password);
            newUser.Major = _majorRepository.GetMajorByMajorId(userModel.MajorId); ;
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
        return _studentRepository.GetByAccountNumber(accountId);
        }

        public IQueryable<User> ListbyStatus(string status)
        {
            return _studentRepository.GetStudentsByStatus(status) as IQueryable<User>;
        }

        public User RejectUser(string accountId)
        {

            var student = Find(accountId);
            if (student != null)
            {
                student.Status = Status.Rejected;
                _studentRepository.Save();
           }
            return student;
        }

        public User ActivateUser(string accountId)
        {
            var student = Find(accountId);
            if (student != null)
            {
                student.Status = Status.Active;
                _studentRepository.Save();
                
            }
            return student;
        }

        public User VerifyUser(string accountId)
        {
            var student = Find(accountId);
            if (student != null)
            {
                student.Status = Status.Verified;
                _studentRepository.Save();
            }
            return student;
        }

        public User DeleteUser(string accountId)
        {
            var user = _studentRepository.DeleteByAccountNumber(accountId);

            return user;
        }

        public IQueryable<User> AllUsers()
        {
            return _studentRepository.GetAll() as IQueryable<User>;
        }

        public int StudentHours(string accountId)
        {
           return _studentRepository.GetStudentHours(accountId);
        }
    }
}