using System.Linq;
using VinculacionBackend.Data.Repositories;


namespace VinculacionBackend.Data.Interfaces
{
    public interface IFacultyRepository
    {
        IQueryable<FacultyProjectCostModel> GetFacultyCosts(long id);
    }
}