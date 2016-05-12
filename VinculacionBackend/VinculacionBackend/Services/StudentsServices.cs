using System;
using System.Linq;
using System.Web;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Enums;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Exceptions;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;

namespace VinculacionBackend.Services
{
    public class StudentsServices : IStudentsServices
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMajorsServices _majorServices;
        private readonly IEncryption _encryption;

        public StudentsServices(IStudentRepository studentRepository, IMajorRepository majorRepository,IEncryption encryption, IMajorsServices majorServices)
        {
            _studentRepository = studentRepository;
            _encryption = encryption;
            _majorServices = majorServices;
        }

        public  User Map(UserEntryModel userModel)
        {
            var newUser = new User();
            newUser.AccountId = userModel.AccountId;
            newUser.Name = userModel.Name;
            newUser.Password = _encryption.Encrypt(userModel.Password);
            newUser.Major = _majorServices.Find(userModel.MajorId);
            newUser.Campus = userModel.Campus;
            newUser.Email = userModel.Email;
            newUser.Status = Status.Inactive;
            newUser.CreationDate = DateTime.Now;
            newUser.ModificationDate = DateTime.Now;
            return newUser;
        }

        public void Add(User user)
        {
            _studentRepository.Insert(user);
            _studentRepository.Save();
            
        }

        public User Find(string accountId)
        {
            var student = _studentRepository.GetByAccountNumber(accountId);
            if(student==null)
                throw new NotFoundException("No se encontro al estudiante");
            return student;
        }

        public IQueryable<User> ListbyStatus(string status)
        {
            return _studentRepository.GetStudentsByStatus(status) as IQueryable<User>;
        }

        public User RejectUser(string accountId)
        {
            var student = Find(accountId);
            if (student == null)
                throw new NotFoundException("No se encontro al estudiante");
            student.Status = Status.Rejected;
            _studentRepository.Save();

            return student;
        }

        public User ActivateUser(string accountId)
        {
            var student = Find(accountId);
            if (student == null)
                throw new NotFoundException("No se encontro al estudiante");
            student.Status = Status.Active;
           _studentRepository.Save();
            return student;
        }

        public User VerifyUser(string accountId)
        {
            var student = Find(accountId);
            if (student == null)
                throw new NotFoundException("No se encontro al estudiante");
            student.Status = Status.Verified;
           _studentRepository.Save();
            
            return student;
        }

       

        public User DeleteUser(string accountId)
        {
            var user = _studentRepository.DeleteByAccountNumber(accountId);
            if(user == null)
                throw new NotFoundException("No se encontro al estudiante");
            _studentRepository.Save();
            return user;
        }

        public IQueryable<User> AllUsers()
        {
            return _studentRepository.GetAll();
        }

        public int GetStudentHours(string accountId)
        {
           return _studentRepository.GetStudentHours(accountId);
        }
    }
}