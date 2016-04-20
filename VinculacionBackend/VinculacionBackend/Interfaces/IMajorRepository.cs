using VinculacionBackend.Entities;
using VinculacionBackend.Repositories;

namespace VinculacionBackend.Interfaces
{
    public interface IMajorRepository : IRepository<Major>
    {
        Major GetMajorByMajorId(string majorId);
    }
}
