using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinculacionBackend.Entities;

namespace VinculacionBackend
{
    interface IProjectRepository : IRepository<Project>
    {
        IEnumerable<User> GetProjectParticipants(long id);
    }
}
