using System.Linq;
using VinculacionBackend.Entities;

namespace VinculacionBackend.Services
{
    public interface IMajorsServices
    {
        Major Find(string majorId);
        IQueryable<Major> All();
    }
}