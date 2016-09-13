using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using VinculacionBackend.Data.Entities;

namespace VinculacionBackend.Interfaces
{
    public interface IMajorsServices
    {
        Major Find(string majorId);
        IQueryable<Major> All();
        IQueryable<Major> GetByProject(long projectId);
        Major FindByName(string name);
    }
}