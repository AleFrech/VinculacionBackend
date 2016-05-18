using VinculacionBackend.Data.Entities;

namespace VinculacionBackend.Data.Interfaces
{
    public interface IMajorRepository : IRepository<Major>
    {
        Major GetMajorByMajorId(string majorId);  
    }
}
