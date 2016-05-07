using VinculacionBackend.Data.Entities;
using VinculacionBackend.Models;

namespace VinculacionBackend.Interfaces
{
    public interface IHoursServices
    {
        Hour Add(HourEntryModel hourModel, string professorUser);
        HourReportModel HourReport(string accountId);
    }
}