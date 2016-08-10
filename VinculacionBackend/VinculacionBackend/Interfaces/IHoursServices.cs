using VinculacionBackend.Data.Entities;
using VinculacionBackend.Models;

namespace VinculacionBackend.Interfaces
{
    public interface IHoursServices
    {
        Hour Add(HourEntryModel hourModel, string professorUser);
        HourReportModel HourReport(string accountId);
        Hour Update(long hourId,HourEntryModel hourModel);
        void AddMany(HourCollectionEntryModel hourModel, string name);
    }
}