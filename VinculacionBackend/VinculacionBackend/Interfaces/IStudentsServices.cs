using System.Data;
using System.Linq;
using System.Net.Http;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Models;

namespace VinculacionBackend.Interfaces
{
    public interface IStudentsServices
    {
        void Map(User student,UserEntryModel userModel);
        void Add(User user);
        User Find(string accountId);
        IQueryable<User> ListbyStatus(string status);
        User RejectUser(string accountId);
        User ActivateUser(string accountId);
        User VerifyUser(string accountId);
        User DeleteUser(string accountId);
        IQueryable<User> AllUsers();
        int GetStudentHours(string accountId);
        HttpResponseMessage GetFiniquitoReport(string accountId);
        User FindByEmail(string email);
        User UpdateStudent(string accountId, UserEntryModel model);
        DataTable[] CreateStudentReport(int year);
        IQueryable<FiniquitoUserModel> GetPendingStudentsFiniquito();
        User GetCurrentStudents(long userId);
        int GetStudentHoursBySection(string accountId, long sectionId);

    }
}
