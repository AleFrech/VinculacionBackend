using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinculacionBackend.Entities;

namespace VinculacionBackend
{
    interface IStudentRepository : IRepository<User>
    {
        User GetByAccountNumber(string accountNumber);
        void DeleteByAccountNumber(string accountNumber);
        int GetStudentHours(string accountNumber);
    }
}
