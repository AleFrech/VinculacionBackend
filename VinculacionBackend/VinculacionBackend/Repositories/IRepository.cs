using System.Collections.Generic;

namespace VinculacionBackend.Repositories
{
    interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(long id);
        T Delete(long id);
        void Save();
        void Update(T ent);
        void Insert(T ent);
    }
}
