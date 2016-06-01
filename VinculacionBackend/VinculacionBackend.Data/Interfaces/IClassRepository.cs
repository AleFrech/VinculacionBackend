using System.Collections.Generic;
using VinculacionBackend.Data.Entities;

namespace VinculacionBackend.Data.Interfaces
{
    public interface IClassRepository : IRepository<Class>
    {
         void InsertClass(Class @class, List<string> majorIds);
    }
}
