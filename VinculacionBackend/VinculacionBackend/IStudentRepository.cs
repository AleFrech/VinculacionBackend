using System.Collections.Generic;
using VinculacionBackend.Entities;
using VinculacionBackend.Enums;

namespace VinculacionBackend.Repositories
{
    interface IStudentRepository : IRepository<User>
    {
        User GetByAccountNumber(string accountNumber);
        User DeleteByAccountNumber(string accountNumber);
        int GetStudentHours(string accountNumber);
        IEnumerable<User> GetStudentsByStatus(Status status);
        IEnumerable<User> GetStudentsByStatus(string status);
    }
}
