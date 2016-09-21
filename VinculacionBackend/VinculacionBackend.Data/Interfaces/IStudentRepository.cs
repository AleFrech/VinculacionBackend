using System.Collections.Generic;
using System.Linq;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Enums;
using VinculacionBackend.Data.Repositories;

namespace VinculacionBackend.Data.Interfaces
{
    public interface IStudentRepository : IRepository<User>
    {
        User GetByAccountNumber(string accountNumber);
        User DeleteByAccountNumber(string accountNumber);
        int GetStudentHours(string accountNumber);
        Dictionary<User, int> GetStudentsHoursByProject(long sectionId, long projectId);
        string GetStudentMajors(List<User> students);
        IEnumerable<User> GetStudentsByStatus(Status status);
        IEnumerable<User> GetStudentsByStatus(string status);
        User GetByEmail(string email);
        int GetStudentHoursByProject(string accountNumber, long projectId);
        StudentCountModel GetStudentCount( string clase, int year);
        StudentCountModel GetStudentByFacultyCount(int faculty, int year);
        StudentCountModel GetHoursCount( string clase, int year);
        StudentCountModel GetHoursByFacultyCount( int faculty, int year);
        IEnumerable<User> GetStudentByMajor(string majorId);
        int GetStudentHoursBySection(string accountId, long sectionId);
        IQueryable<object> GetStudentSections(string accountId);
        void InsertMany(IList<User> students);
    }
}
