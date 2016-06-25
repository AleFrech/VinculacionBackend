using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinculacionBackend.Data.Database;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;

namespace VinculacionBackend.Data.Repositories
{
    public class FacultyRepository : IFacultyRepository
    {
        private readonly VinculacionContext _context;

        public FacultyRepository(VinculacionContext context)
        {
            _context = context;
        }

        public IQueryable<FacultyProjectCostModel> GetFacultyCosts(long id)
        {
            return
                _context.ProjectMajorRels.Where(f => f.Major.Faculty.Id == id)
                    .Select(
                        x =>
                            new FacultyProjectCostModel
                            {
                                FacultyName = x.Major.Faculty.Name,
                                ProjectCost = x.Project.Cost
                            });
        } 
    }
}
