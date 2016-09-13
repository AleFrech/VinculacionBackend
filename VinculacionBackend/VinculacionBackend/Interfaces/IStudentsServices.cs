using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using ClosedXML.Excel;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Models;
using VinculacionBackend.Services;

namespace VinculacionBackend.Interfaces
{
    public interface IStudentsServices
    {
        void Map(User student,UserEntryModel userModel);
        IQueryable<object> ParseExcelStudents(XLWorkbook excel);
        void Add(User user);
        User Find(string accountId);
        IQueryable<User> ListbyStatus(string status);
        User ActivateUser(string accountId);
        User DeleteUser(string accountId);
        IQueryable<User> AllUsers();
        int GetStudentHours(string accountId);
        HttpResponseMessage GetFiniquitoReport(string accountId);
        User FindByEmail(string email);
        User UpdateStudent(string accountId, UserUpdateModel model);
        List<StudentReportModel> CreateStudentReport(int year);
        IQueryable<FiniquitoUserModel> GetPendingStudentsFiniquito();
        User GetCurrentStudents(long userId);
        int GetStudentHoursBySection(string accountId, long sectionId);
        IQueryable<object> GetStudentSections(string accountId);
        void AddMany(IList<StudentAddManyEntryModel> students);
        void ChangePassword(StudentChangePasswordModel model);

        List<StudentReportModel> CreateHourNumberReport(int year);
    }
}
