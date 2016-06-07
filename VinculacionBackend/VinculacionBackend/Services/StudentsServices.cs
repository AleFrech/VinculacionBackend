using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IProjectServices _projectServices;
        private readonly IEncryption _encryption;

        public StudentsServices(IStudentRepository studentRepository, IMajorRepository majorRepository,IEncryption encryption, IMajorsServices majorServices, IProjectServices projectServices)
        {
            _studentRepository = studentRepository;
            _encryption = encryption;
            _majorServices = majorServices;
            _projectServices = projectServices;
        }

        public  void Map(User student,UserEntryModel userModel)
        {         
            student.AccountId = userModel.AccountId;
            student.Name = userModel.Name;
            student.Password = _encryption.Encrypt(userModel.Password);
            student.Major = _majorServices.Find(userModel.MajorId);
            student.Campus = userModel.Campus;
            student.Email = userModel.Email;
            student.Status = Status.Inactive;
            student.CreationDate = DateTime.Now;
            student.ModificationDate = DateTime.Now;
        }


        public void PutMap(User student, UserEntryModel userModel)
        {
            student.AccountId = userModel.AccountId;
            student.Name = userModel.Name;
            student.Password = _encryption.Encrypt(userModel.Password);
            if (student.Major.MajorId != userModel.MajorId)
                student.Major = _majorServices.Find(userModel.MajorId);
            student.Campus = userModel.Campus;
            student.Email = userModel.Email;
            student.ModificationDate = DateTime.Now;
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
            if (user == null)
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

        public User FindByEmail(string email)
        {
            var student = _studentRepository.GetByEmail(email);
            if (student == null)
                throw new NotFoundException("No se encontro al estudiante");
            return student;
        }

        public User UpdateStudent(string accountId, UserEntryModel model)
        {
            var student = _studentRepository.GetByAccountNumber(accountId);
            if (student == null)
                throw new NotFoundException("No se encontro al estudiante");
            PutMap(student, model);

            _studentRepository.Update(student);
            _studentRepository.Save();
            return student;
        }

        private Dictionary<Student, int> GetStudentsHoursByProject(int projectId)
        {
            var projectStudents = _projectServices.GetProjectStudents(projectId);
            Dictionary<Student, int> studentHours = new Dictionary<Student, int>();

            foreach(var projectStudent in projectStudents)
            {
                var hours = _studentRepository.GetStudentHoursByProject(projectStudent.AccountId, projectId);
                studentHours[projectStudent] = hours; 
            }

            return studentHours;
        }

    }
}