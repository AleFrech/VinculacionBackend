using VinculacionBackend.Data.Entities;

namespace VinculacionBackend.Data.Interfaces
{
    public interface ISectionRepository : IRepository<Section>
    {
        void AssignStudent(long sectionId, long studentId);
    }
}
