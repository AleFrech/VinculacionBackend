using VinculacionBackend.Data.Entities;

namespace VinculacionBackend.Data.Interfaces
{
    public interface IPeriodRepository : IRepository<Period>
    {
        Period GetCurrent();
    }
}