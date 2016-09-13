using System.Collections.Generic;
using System.Data;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Models;
using VinculacionBackend.Services;

namespace VinculacionBackend.Interfaces
{
    public interface IFacultiesServices
    {
        Dictionary<string, List<PeriodCostModel>> GetFacultiesCosts(Faculty faculty,int year);
        List<FacultyCostsReportEntry> CreateFacultiesCostReport(int year);
        Dictionary<string, int> GetFacultiesHours(int year);
        List<FacultyHoursReportEntryModel> CreateFacultiesHourReport(int year);
    }
}