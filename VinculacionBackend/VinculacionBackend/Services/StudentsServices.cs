using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Ajax.Utilities;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Enums;
using VinculacionBackend.Data.Exceptions;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;
using VinculacionBackend.Reports;

namespace VinculacionBackend.Services
{
    public class StudentReportModel
    {
        public string StudentNumber { get; set; }
        public int FirstPeriod { get; set; }
        public int SecondPeriod { get; set; }
        public int FourthPeriod { get; set; }
        public int FifthPeriod { get; set; }
    }
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


        public void PutMap(User student, UserUpdateModel userModel)
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
            if (student == null)
                throw new NotFoundException("No se encontro el estudiante");
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

        public User UpdateStudent(string accountId, UserUpdateModel model)
        {
            var student = _studentRepository.GetByAccountNumber(accountId);
            if (student == null)
                throw new NotFoundException("No se encontro al estudiante");
            PutMap(student, model);

            _studentRepository.Update(student);
            _studentRepository.Save();
            return student;
        }

        public List<StudentReportModel> CreateStudentReport(int year)
        {
            var report = new List<StudentReportModel>();

            var count = _studentRepository.GetStudentCount( "Inglés", year);
            report.Add(new StudentReportModel { StudentNumber = "Inglés", FirstPeriod = count.FirstPeriod ,SecondPeriod = count.SecondPeriod,FourthPeriod = count.FourthPeriod,FifthPeriod = count.FifthPeriod });
            count = _studentRepository.GetStudentCount("Ofimática", year);
            report.Add(new StudentReportModel { StudentNumber = "Ofimática", FirstPeriod = count.FirstPeriod, SecondPeriod = count.SecondPeriod, FourthPeriod = count.FourthPeriod, FifthPeriod = count.FifthPeriod });
            count = _studentRepository.GetStudentCount("Sociología", year);
            report.Add(new StudentReportModel { StudentNumber = "Sociología", FirstPeriod = count.FirstPeriod, SecondPeriod = count.SecondPeriod, FourthPeriod = count.FourthPeriod, FifthPeriod = count.FifthPeriod });
            count = _studentRepository.GetStudentCount("Filosofía", year);
            report.Add(new StudentReportModel { StudentNumber = "Filosofía", FirstPeriod = count.FirstPeriod, SecondPeriod = count.SecondPeriod, FourthPeriod = count.FourthPeriod, FifthPeriod = count.FifthPeriod });
            count = _studentRepository.GetStudentCount("Ecología", year);
            report.Add(new StudentReportModel { StudentNumber = "Ecología", FirstPeriod = count.FirstPeriod, SecondPeriod = count.SecondPeriod, FourthPeriod = count.FourthPeriod, FifthPeriod = count.FifthPeriod });
            count = _studentRepository.GetStudentByFacultyCount(1, year);
            report.Add(new StudentReportModel { StudentNumber = "FIA", FirstPeriod = count.FirstPeriod, SecondPeriod = count.SecondPeriod, FourthPeriod = count.FourthPeriod, FifthPeriod = count.FifthPeriod });
            count = _studentRepository.GetStudentByFacultyCount(2, year);
            report.Add(new StudentReportModel { StudentNumber = "FCAS", FirstPeriod = count.FirstPeriod, SecondPeriod = count.SecondPeriod, FourthPeriod = count.FourthPeriod, FifthPeriod = count.FifthPeriod });

            var sum = new StudentReportModel { StudentNumber = "Total"};
            foreach (StudentReportModel model in report)
            {
                sum.FirstPeriod += model.FirstPeriod;
                sum.SecondPeriod += model.SecondPeriod;
                sum.FourthPeriod += model.FourthPeriod;
                sum.FifthPeriod += model.FifthPeriod;
            }
            report.Add(sum);

            return report;
        }

        public List<StudentReportModel> CreateHourNumberReport(int year)
        {
            var report = new List<StudentReportModel>();

            var count = _studentRepository.GetHoursCount("Inglés", year);
            report.Add(new StudentReportModel { StudentNumber = "Inglés", FirstPeriod = count.FirstPeriod, SecondPeriod = count.SecondPeriod, FourthPeriod = count.FourthPeriod, FifthPeriod = count.FifthPeriod });
            count = _studentRepository.GetHoursCount("Ofimática", year);
            report.Add(new StudentReportModel { StudentNumber = "Ofimática", FirstPeriod = count.FirstPeriod, SecondPeriod = count.SecondPeriod, FourthPeriod = count.FourthPeriod, FifthPeriod = count.FifthPeriod });
            count = _studentRepository.GetHoursCount("Sociología", year);
            report.Add(new StudentReportModel { StudentNumber = "Sociología", FirstPeriod = count.FirstPeriod, SecondPeriod = count.SecondPeriod, FourthPeriod = count.FourthPeriod, FifthPeriod = count.FifthPeriod });
            count = _studentRepository.GetHoursCount("Filosofía", year);
            report.Add(new StudentReportModel { StudentNumber = "Filosofía", FirstPeriod = count.FirstPeriod, SecondPeriod = count.SecondPeriod, FourthPeriod = count.FourthPeriod, FifthPeriod = count.FifthPeriod });
            count = _studentRepository.GetHoursCount("Ecología", year);
            report.Add(new StudentReportModel { StudentNumber = "Ecología", FirstPeriod = count.FirstPeriod, SecondPeriod = count.SecondPeriod, FourthPeriod = count.FourthPeriod, FifthPeriod = count.FifthPeriod });
            count = _studentRepository.GetHoursByFacultyCount(1, year);
            report.Add(new StudentReportModel { StudentNumber = "FIA", FirstPeriod = count.FirstPeriod, SecondPeriod = count.SecondPeriod, FourthPeriod = count.FourthPeriod, FifthPeriod = count.FifthPeriod });
            count = _studentRepository.GetHoursByFacultyCount(2, year);
            report.Add(new StudentReportModel { StudentNumber = "FCAS", FirstPeriod = count.FirstPeriod, SecondPeriod = count.SecondPeriod, FourthPeriod = count.FourthPeriod, FifthPeriod = count.FifthPeriod });

            var sum = new StudentReportModel { StudentNumber = "Total" };
            foreach (StudentReportModel model in report)
            {
                sum.FirstPeriod += model.FirstPeriod;
                sum.SecondPeriod += model.SecondPeriod;
                sum.FourthPeriod += model.FourthPeriod;
                sum.FifthPeriod += model.FifthPeriod;
            }
            report.Add(sum);

            return report;
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

        public void AddMany(IList<StudentAddManyEntryModel> entries)
        {
            entries.Select(entry => new User
            {
                Name = entry.Name,
                AccountId = entry.AccountId,
                Major = _majorServices.Find(entry.Major),
                Email = entry.Email, Password = _encryption.Encrypt("12345"),
                Campus = "SPS",
                CreationDate = DateTime.Now,
                ModificationDate = DateTime.Now,
                Finiquiteado = false,
                Status = Status.Inactive
            }).ToList().ForEach(Add);
        }

        public DataTable FromExcelToDataTable(XLWorkbook excel)
        {
            var dataTable = new DataTable();
            var firstRow = true;
            foreach (IXLRow row in excel.Worksheet(1).Rows())
            {
                if (firstRow)
                {
                    dataTable.Columns.Add("AccountId");
                    dataTable.Columns.Add("Name");
                    dataTable.Columns.Add("Email");
                    dataTable.Columns.Add("Major");
                    dataTable.Columns.Add("Exists");
                    firstRow = false;
                }
                else
                {
                    dataTable.Rows.Add();
                    int pos = 0;
                    foreach (IXLCell cell in row.Cells())
                    {
                        dataTable.Rows[dataTable.Rows.Count - 1][pos] = cell.Value.ToString();
                        pos++;
                    }
                    var accountNumber = dataTable.Rows[0][0].ToString();
                    dataTable.Rows[dataTable.Rows.Count - 1][pos] = _studentRepository.GetAll().Any(x => x.AccountId == accountNumber);

                }
            }
            return dataTable;
        }
        public IQueryable<object> ParseExcelStudents(XLWorkbook excel)
        {
            var dataTable = FromExcelToDataTable(excel);
          
            var results = from row in dataTable.AsEnumerable()
                          select new
                          {
                              AccountId = row.Field<string>("AccountId"),
                              Name = row.Field<string>("Name"),
                              Email = row.Field<string>("Email"),
                              Major = row.Field<string>("Major"),
                              Exists = Convert.ToBoolean(row.Field<string>("Exists"))
                          };
            return results.AsQueryable();
        }
    }
}
