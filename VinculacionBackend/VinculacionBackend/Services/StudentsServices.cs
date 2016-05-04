using System;
using System.CodeDom;
using System.Linq;
using System.Web;
using VinculacionBackend.ActionFilters;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Enums;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;
using VinculacionBackend.Security.Interfaces;

namespace VinculacionBackend.Services
{
    public class StudentsServices : IStudentsServices
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMajorRepository _majorRepository;
        private readonly IEncryption _encryption;

        public StudentsServices(IStudentRepository studentRepository, IMajorRepository majorRepository,IEncryption encryption)
        {
            _studentRepository = studentRepository;
            _majorRepository = majorRepository;
            _encryption = encryption;
        }

        public  User Map(UserEntryModel userModel)
        {
            var newUser = new User();
            newUser.AccountId = userModel.AccountId;
            newUser.Name = userModel.Name;
            newUser.Password = _encryption.Encrypt(userModel.Password);
            newUser.Major = _majorRepository.GetMajorByMajorId(userModel.MajorId);
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
               throw new NotFoundException("No se encontro el alumno");
            return student;
        }

        public IQueryable<User> ListbyStatus(string status)
        {
            return _studentRepository.GetStudentsByStatus(status) as IQueryable<User>;
        }

        public User RejectUser(string accountId)
        {

            var student = Find(accountId);    
            student.Status = Status.Rejected;
           _studentRepository.Save();
      
            return student;
        }

        public User ActivateUser(string accountId)
        {
            var student = Find(accountId);
           student.Status = Status.Active;
          _studentRepository.Save();
      
            return student;
        }

        public User VerifyUser(string accountId)
        {
            var student = Find(accountId);
            student.Status = Status.Verified;
            _studentRepository.Save();
            return student;
        }

       

        public User DeleteUser(string accountId)
        {
            var user = _studentRepository.DeleteByAccountNumber(accountId);
            _studentRepository.Save();
            return user;
        }

        public IQueryable<User> AllUsers()
        {
            return _studentRepository.GetAll();
        }

        public int GetStudentHours(string accountId)
        {
           var total = _studentRepository.GetStudentHours(accountId);
            if (total == 0)
                throw new NotFoundException("No se encontro registro de horas para dicho estudiante");
            return total;
        }
    }
}