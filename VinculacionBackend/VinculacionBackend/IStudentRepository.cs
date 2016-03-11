using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinculacionBackend.Entities;
using VinculacionBackend.Enums;

namespace VinculacionBackend
{
    interface IStudentRepository : IRepository<User>
    {
        User GetByAccountNumber(string accountNumber);
        void DeleteByAccountNumber(string accountNumber);
        int GetStudentHours(string accountNumber);
        IEnumerable<User> GetStudentsByStatus(Status status);
        IEnumerable<User> GetStudentsByStatus(string status);
    }
}
