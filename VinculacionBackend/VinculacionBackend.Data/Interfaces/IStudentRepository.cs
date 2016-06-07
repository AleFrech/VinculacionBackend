using System.Collections.Generic;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Enums;

namespace VinculacionBackend.Data.Interfaces
{
    public interface IStudentRepository : IRepository<User>
    {
        User GetByAccountNumber(string accountNumber);
        User DeleteByAccountNumber(string accountNumber);
        int GetStudentHours(string accountNumber);
        IEnumerable<User> GetStudentsByStatus(Status status);
        IEnumerable<User> GetStudentsByStatus(string status);
        User GetByEmail(string email);
        int GetStudentHoursByProject(string accountNumber, int projectId);
    }
}
