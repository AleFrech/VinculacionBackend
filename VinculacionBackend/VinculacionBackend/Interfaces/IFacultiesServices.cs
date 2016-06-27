using System.Collections.Generic;
using System.Data;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Services;

namespace VinculacionBackend.Interfaces
{
    public interface IFacultiesServices
    {
        Dictionary<string, List<PeriodCostModel>> GetFacultiesCosts(Faculty faculty,int year);
        DataTable CreateFacultiesCostReport();
        Dictionary<string, int> GetFacultiesHours();
        DataTable CreateFacultiesHourReport();
    }
}