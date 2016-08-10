using System.Collections.Generic;
using System.Linq;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Enums;

namespace VinculacionBackend.Data.Interfaces
{
    public interface IStudentRepository : IRepository<User>
    {
        User GetByAccountNumber(string accountNumber);
        User DeleteByAccountNumber(string accountNumber);
        int GetStudentHours(string accountNumber);
        Dictionary<User, int> GetStudentsHoursByProject(long projectId);
        string GetStudentMajors(List<User> students);
        IEnumerable<User> GetStudentsByStatus(Status status);
        IEnumerable<User> GetStudentsByStatus(string status);
        User GetByEmail(string email);
        int GetStudentHoursByProject(string accountNumber, long projectId);
        int GetStudentCount(int periodo, string clase, int year);
        int GetStudentByFacultyCount(int period, int faculty, int year);
        int GetHoursCount(int period, string clase, int year);
        int GetHoursByFacultyCount(int period, int faculty, int year);
        IEnumerable<User> GetStudentByMajor(string majorId);
        int GetStudentHoursBySection(string accountId, long sectionId);
        IQueryable<object> GetStudentSections(string accountId);
    }
}
