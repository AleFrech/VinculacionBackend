using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Models;

namespace VinculacionBackend.Interfaces
{
    public interface IClassesServices
    {
        IQueryable<Class> All();
        Class Delete(long id);
        void Add(Class @class);
        Class Find(long id);
        void Map(Class @class, ClassEntryModel classModel);
        Class UpdateClass(long classId, ClassEntryModel classModel);
    }
}
