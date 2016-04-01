using VinculacionBackend.Entities;

namespace VinculacionBackend.Repositories
{
    public interface IMajorRepository : IRepository<Major>
    {
        Major GetMajorByMajorId(string majorId);
    }
}
