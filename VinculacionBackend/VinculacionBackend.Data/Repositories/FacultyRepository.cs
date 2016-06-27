using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using VinculacionBackend.Data.Database;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Data.Models;

namespace VinculacionBackend.Data.Repositories
{
    public class FacultyRepository : IFacultyRepository
    {
        private readonly VinculacionContext _context;

        public FacultyRepository()
        {
            _context = new VinculacionContext();
        }

        public List<FacultyProjectCostModel> GetFacultyCosts(int id,int period,int year)
        {
            List<FacultyProjectCostModel> facultyCostModelList = new List<FacultyProjectCostModel>();
            var projectsIds = _context.SectionProjectsRels.Where(x => x.Section.Period.Number == period && x.Section.Period.Year == year).Select(x => x.Project.Id).ToList();
            foreach (var p in projectsIds)
            {
                var result = _context.ProjectMajorRels.Where(x => x.Major.Faculty.Id == id && x.Project.Id == p).Where(x=>x.Project!=null).Select(x => new FacultyProjectCostModel
                {
                    FacultyName = x.Major.Faculty.Name,
                    ProjectCost = x.Project.Cost
                }).Distinct();
                facultyCostModelList.AddRange(result.ToList());
            }

            return facultyCostModelList;

        }
        public IQueryable<Faculty> GetAll()
        {
            return _context.Faculties;
        }

    }
}
