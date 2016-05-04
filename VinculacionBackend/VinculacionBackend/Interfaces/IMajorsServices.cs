using System.Linq;
using VinculacionBackend.Data.Entities;

namespace VinculacionBackend.Interfaces
{
    public interface IMajorsServices
    {
        Major Find(string majorId);
        IQueryable<Major> All();
    }
}