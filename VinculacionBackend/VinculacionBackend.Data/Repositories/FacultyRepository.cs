using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
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

        public List<FacultyProjectCostModel> GetFacultyCosts(long id,int period,int year)
        {
            List<FacultyProjectCostModel> facultyCostModelList = new List<FacultyProjectCostModel>();
            var projectsIds = _context.SectionProjectsRels.Where(x => x.Section.Period.Number == period && x.Section.Period.Year == year).Include(z => z.Project).ToList();
            foreach (var p in projectsIds)
            {
                var result = _context.ProjectMajorRels.Where(x => x.Major.Faculty.Id == id && x.Project.Id == p.Project.Id).Where(x=>x.Project!=null).Select(x => new FacultyProjectCostModel
                {
                    FacultyName = x.Major.Faculty.Name,
                    ProjectCost = p.Cost
                }).Distinct();
                facultyCostModelList.AddRange(result.ToList());
            }

            return facultyCostModelList;

        }
        public IQueryable<Faculty> GetAll()
        {
            return _context.Faculties;
        }
        //TODO: Pasar a un query
        public int GetFacultyHours(long id, int year)
        {
            var projectsIds = _context.SectionProjectsRels.Where(x => x.Section.Period.Year == year).Select(x => x.Id).ToList();
            var majors = _context.Majors.Where(x => x.Faculty.Id == id).Select(x=>x.MajorId).ToList();
            var users = _context.Users.Where(x => majors.Any(y => y == x.Major.MajorId)).Select(x => x.Id).ToList();
            var hours = _context.Hours.Where(x => projectsIds.Contains(x.SectionProject.Id) && users.Contains(x.User.Id)).Select(x=>x.Amount);
            var total = 0;
            foreach (var hour in hours)
            {
                total += hour;
            }
            return total;
        }

    }
}
