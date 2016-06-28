using System.Collections.Generic;
using System.Data;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Services;

namespace VinculacionBackend.Interfaces
{
    public interface IFacultiesServices
    {
        Dictionary<string, List<PeriodCostModel>> GetFacultiesCosts(Faculty faculty,int year);
        DataTable CreateFacultiesCostReport(int year);
        Dictionary<string, int> GetFacultiesHours(int year);
        DataTable CreateFacultiesHourReport(int year);
    }
}