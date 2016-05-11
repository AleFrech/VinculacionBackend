using VinculacionBackend.Data.Entities;

namespace VinculacionBackend.Data.Interfaces
{
    public interface ISectionRepository : IRepository<Section>
    {
        void AssignStudent(long sectionId, long studentId);
        void RemoveStudent(long sectionId, long studentId);
    }
}
