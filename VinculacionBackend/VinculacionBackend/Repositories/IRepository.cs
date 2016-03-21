using System.Collections.Generic;
using System.Linq;
using VinculacionBackend.Entities;

namespace VinculacionBackend.Repositories
{
    interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T Get(long id);
        T Delete(long id);
        void Save();
        void Update(T ent);
        void Insert(T ent);
    }
}
