using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinculacionBackend.Data.Entities;

namespace VinculacionBackend.Services
{
    public interface IUsersServices
    {
        User Find(string user, string password);
    }
}
