using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using Microsoft.Ajax.Utilities;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Enums;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Exceptions;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;
using VinculacionBackend.Reports;

namespace VinculacionBackend.Services
{
    public class StudentsServices : IStudentsServices
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMajorsServices _majorServices;
        private readonly IEncryption _encryption;
        private readonly ITextDocumentServices _textDocumentServices;
        private readonly IHourRepository _hourRepository;

        public StudentsServices(IStudentRepository studentRepository, IEncryption encryption, IMajorsServices majorServices, ITextDocumentServices textDocumentServices, IHourRepository hourRepository)
        {
            _studentRepository = studentRepository;
            _encryption = encryption;
            _majorServices = majorServices;
            _textDocumentServices = textDocumentServices;
            _hourRepository = hourRepository;
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
            student.Finiquiteado = false;
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
            student.Finiquiteado = false;
        }

        public void ChangePassword(StudentChangePasswordModel model)
        {
            var student=_studentRepository.GetByAccountNumber(model.AccountId);
            student.Password = _encryption.Encrypt(model.Password);
            _studentRepository.Update(student);
            _studentRepository.Save();
        }


        public void Add(User user)
        {
            _studentRepository.Insert(user);
            _studentRepository.Save();
        }


        public HttpResponseMessage GetFiniquitoReport(string accountId)
        {
            var finiquitoReport= new FiniquitoReport(_textDocumentServices,_studentRepository,new DownloadbleFile());
            return finiquitoReport.GenerateFiniquitoReport(accountId);
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


        public User ActivateUser(string accountId)
        {
            var student = Find(accountId);
            student.Status = Status.Active;
            _studentRepository.Update(student);
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

        public DataTable[] CreateStudentReport(int year)
        {
            var dt = new DataTable();
            dt.Columns.Add("Numero de alumnos", typeof(string));
            dt.Columns.Add("1er Periodo", typeof(int));
            dt.Columns.Add("2do Periodo", typeof(int));
            dt.Columns.Add("3er Periodo", typeof(int));
            dt.Columns.Add("4to Periodo", typeof(int));

            dt.Rows.Add("Inglés", _studentRepository.GetStudentCount(1, "Inglés", year), _studentRepository.GetStudentCount(2, "Inglés", year), _studentRepository.GetStudentCount(4, "Inglés", year), _studentRepository.GetStudentCount(5, "Inglés", year));
            dt.Rows.Add("Ofimática", _studentRepository.GetStudentCount(1, "Ofimática", year), _studentRepository.GetStudentCount(2, "Ofimática", year), _studentRepository.GetStudentCount(4, "Ofimática", year), _studentRepository.GetStudentCount(5, "Ofimática", year));
            dt.Rows.Add("Sociología", _studentRepository.GetStudentCount(1, "Sociología", year), _studentRepository.GetStudentCount(2, "Sociología", year), _studentRepository.GetStudentCount(4, "Sociología", year), _studentRepository.GetStudentCount(5, "Sociología", year));
            dt.Rows.Add("Filosofía", _studentRepository.GetStudentCount(1, "Filosofía", year), _studentRepository.GetStudentCount(2, "Filosofía", year), _studentRepository.GetStudentCount(4, "Filosofía", year), _studentRepository.GetStudentCount(5, "Filosofía", year));
            dt.Rows.Add("Ecología", _studentRepository.GetStudentCount(1, "Ecología", year), _studentRepository.GetStudentCount(2, "Ecología", year), _studentRepository.GetStudentCount(4, "Ecología", year), _studentRepository.GetStudentCount(5, "Ecología", year));
            dt.Rows.Add("FIA", _studentRepository.GetStudentByFacultyCount(1, 1, year), _studentRepository.GetStudentByFacultyCount(2, 1, year), _studentRepository.GetStudentByFacultyCount(4, 1, year), _studentRepository.GetStudentByFacultyCount(5, 1, year));
            dt.Rows.Add("FCAS", _studentRepository.GetStudentByFacultyCount(1, 2, year), _studentRepository.GetStudentByFacultyCount(2, 2, year), _studentRepository.GetStudentByFacultyCount(4, 2, year), _studentRepository.GetStudentByFacultyCount(5, 2, year));
            dt.Rows.Add();

            int[] sum = new int[4];
            foreach (DataRow dr in dt.Rows)
            {
                int col = 0;
                foreach (DataColumn dc in dt.Columns)
                {
                    var value = dr[dc];
                    if (!dc.ColumnName.Equals("Numero de alumnos") && value is int)
                    {
                        sum[col++] += (int)value;
                    }
                }
            }
            dt.Rows.Add("Total", sum[0], sum[1], sum[2], sum[3]);

            var dataTables = new DataTable[2];
            dataTables[0] = dt;

            var dt2 = new DataTable();
            dt2.Columns.Add("Numero de Horas", typeof(string));
            dt2.Columns.Add("1er Periodo", typeof(int));
            dt2.Columns.Add("2do Periodo", typeof(int));
            dt2.Columns.Add("3er Periodo", typeof(int));
            dt2.Columns.Add("4to Periodo", typeof(int));

            dt2.Rows.Add("Inglés", _studentRepository.GetHoursCount(1, "Inglés", year), _studentRepository.GetHoursCount(2, "Inglés", year), _studentRepository.GetHoursCount(4, "Inglés", year), _studentRepository.GetHoursCount(5, "Inglés", year));
            dt2.Rows.Add("Ofimática", _studentRepository.GetHoursCount(1, "Ofimática", year), _studentRepository.GetHoursCount(2, "Ofimática", year), _studentRepository.GetHoursCount(4, "Ofimática", year), _studentRepository.GetHoursCount(5, "Ofimática", year));
            dt2.Rows.Add("Sociología", _studentRepository.GetHoursCount(1, "Sociología", year), _studentRepository.GetHoursCount(2, "Sociología", year), _studentRepository.GetHoursCount(4, "Sociología", year), _studentRepository.GetHoursCount(5, "Sociología", year));
            dt2.Rows.Add("Filosofía", _studentRepository.GetHoursCount(1, "Filosofía", year), _studentRepository.GetHoursCount(2, "Filosofía", year), _studentRepository.GetHoursCount(4, "Filosofía", year), _studentRepository.GetHoursCount(5, "Filosofía", year));
            dt2.Rows.Add("Ecología", _studentRepository.GetHoursCount(1, "Ecología", year), _studentRepository.GetHoursCount(2, "Ecología", year), _studentRepository.GetHoursCount(4, "Ecología", year), _studentRepository.GetHoursCount(5, "Ecología", year));
            dt2.Rows.Add("FIA", _studentRepository.GetHoursByFacultyCount(1, 1, year), _studentRepository.GetHoursByFacultyCount(2, 1, year), _studentRepository.GetHoursByFacultyCount(4, 1, year), _studentRepository.GetHoursByFacultyCount(5, 1, year));
            dt2.Rows.Add("FCAS", _studentRepository.GetHoursByFacultyCount(1, 2, year), _studentRepository.GetHoursByFacultyCount(2, 2, year), _studentRepository.GetHoursByFacultyCount(4, 2, year), _studentRepository.GetHoursByFacultyCount(5, 2, year));
            dt2.Rows.Add();

            sum = new int[4];
            foreach (DataRow dr in dt2.Rows)
            {
                int col = 0;
                foreach (DataColumn dc in dt2.Columns)
                {
                    var value = dr[dc];
                    if (!dc.ColumnName.Equals("Numero de Horas") && value is int)
                    {
                        sum[col++] += (int)value;
                    }
                }
            }
            dt2.Rows.Add("Total", sum[0], sum[1], sum[2], sum[3]);

            dataTables[1] = dt2;
            return dataTables;
        }

        public IQueryable<FiniquitoUserModel> GetPendingStudentsFiniquito()
        {
            var students = _studentRepository.GetAll().ToList();
            var hours = _hourRepository.GetAll().ToList();

            var toReturn = new List<FiniquitoUserModel>();

            foreach (var student in students)
            {
                int hourTotal = 0;
                bool validYear = false;
                foreach(var hour in hours)
                {
                    if (hour.User.Id == student.Id)
                    {
                        hourTotal += hour.Amount;
                        if (hour.SectionProject.Section.Period.Year >= 2016)
                            validYear = true;
                    }
                }

                if (hourTotal >= 100 && !student.Finiquiteado && validYear)
                {
                    toReturn.Add(new FiniquitoUserModel
                    {
                        Id = student.Id, AccountId =  student.AccountId, Major =  student.Major,
                        Name =  student.Name, Campus = student.Campus, CreationDate = student.CreationDate,
                        Email = student.Email, Finiquiteado = student.Finiquiteado, ModificationDate = student.ModificationDate,
                        Password = student.Password, Status =  student.Status, Hours = hourTotal
                    
                    });
                }
            }

            return toReturn.AsQueryable();
        }

        public User GetCurrentStudents(long userId)
        {
            return _studentRepository.Get(userId);
        }

        public int GetStudentHoursBySection(string accountId, long sectionId)
        {
            return _studentRepository.GetStudentHoursBySection(accountId, sectionId);
        }

        public IQueryable<object> GetStudentSections(string accountId)
        {
            return _studentRepository.GetStudentSections(accountId);
        }

        public void AddMany(IList<User> students)
        {
            students.ForEach(Add);
        }
    }
}