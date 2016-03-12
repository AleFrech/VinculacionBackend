using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using VinculacionBackend.Entities;
using VinculacionBackend.Models;

namespace VinculacionBackend.Services
{
    interface IUsersServices
    {
        User Map(User user, UserEntryModel userModel);
        void Add(User user);
        User Find(string accountId);
        IQueryable<User> ListbyStatus(string status);
        User RejectUser(string accountId,string message);
        Boolean AcceptUser(string accountId);
        User DeleteUser(string accountId);


    }
}
