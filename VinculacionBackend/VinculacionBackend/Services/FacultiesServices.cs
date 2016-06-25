using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Spire.Pdf.Exporting.XPS.Schema;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Data.Repositories;

namespace VinculacionBackend.Services
{
    public class FacultiesServices
    {
        IFacultyRepository _facultyRepository;

        public FacultiesServices(IFacultyRepository facultyRepository)
        {
            _facultyRepository = facultyRepository;
        }

        public Dictionary<string, float> GetFacultiesCosts()
        {
            Dictionary<string,float> FacultiesCosts = new Dictionary<string, float>();
            for (int i = 1; i < 3; i++)
            {
                var FacultyCosts = _facultyRepository.GetFacultyCosts(i).ToList();
                var TotalCosts = FacultyCosts.Sum(fc => (float) fc.ProjectCost);
                FacultiesCosts.Add(FacultyCosts.ElementAt(i-1).FacultyName,TotalCosts);
            }
            return FacultiesCosts;
        }
    }
}