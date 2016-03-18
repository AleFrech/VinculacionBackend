using VinculacionBackend.Entities;

namespace VinculacionBackend.Repositories
{
    interface IMajorRepository : IRepository<Major>
    {
        Major GetMajorByMajorId(string majorId);
    }
}
