using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinculacionBackend.Data.Entities;

namespace VinculacionBackend.Interfaces
{
    public interface IUsersServices
    {
        User FindValidUser(string user, string password);
        string GetUserRole(string email);
    }
}
