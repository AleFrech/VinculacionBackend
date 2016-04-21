using System.Linq;

namespace VinculacionBackend.Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T Get(long id);
        T Delete(long id);
        void Save();
        void Update(T ent);
        void Insert(T ent);
    }
}
