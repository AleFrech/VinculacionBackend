using System.Collections.Generic;
using System.Linq;
using VinculacionBackend.Data.Entities;

namespace VinculacionBackend.Data.Interfaces
{
    public interface IMajorRepository : IRepository<Major>
    {
        Major GetMajorByMajorId(string majorId);
        IQueryable<Major> GetMajorsByFaculty(long facultyId);
        IQueryable<Major> GetMajorsByProject(long projectId);
        Major GetMajorByName(string name);
    }
}
