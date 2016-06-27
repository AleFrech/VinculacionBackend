using System.Collections.Generic;
using System.Data;
using VinculacionBackend.Services;

namespace VinculacionBackend.Interfaces
{
    public interface IFacultiesServices
    {
        Dictionary<string, List<PeriodCostModel>> GetFacultiesCosts(int i,int year);
        DataTable CreateFacultiesCostReport();
        Dictionary<string, int> GetFacultiesHours();
    }
}