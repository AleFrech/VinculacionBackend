using System.Collections.Generic;
using System.Linq;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;

namespace VinculacionBackend.Services
{
    public class HoursServices : IHoursServices
    {
        private readonly IHourRepository _hourRepository;

        public HoursServices(IHourRepository hourRepository)
        {
            this._hourRepository = hourRepository;
        }

        public Hour Add(HourEntryModel hourModel,string professorUser)
        {
            var hour =_hourRepository.InsertHourFromModel(hourModel.AccountId, hourModel.SectionId, hourModel.ProjectId, hourModel.Hour,professorUser);
            _hourRepository.Save();
            return hour;
        }

        public HourReportModel HourReport(string accountId)
        {
            var hours = _hourRepository.GetStudentHours(accountId);
            var totalHours = hours.Sum(hour => (int?)hour.Amount) ?? 0;
            var reportProject = new List<HourReportUnitModel>();
            foreach (var hour in hours)
            {
                var project = new HourReportUnitModel
                {
                    ProjectId = hour.SectionProject.Project.ProjectId,
                    ProjectName = hour.SectionProject.Project.Name,
                    SectionName = hour.SectionProject.Section != null ? hour.SectionProject.Section.Code : "",
                    HoursWorked = hour.Amount,
                    ProjectDescription = hour.SectionProject.Project.Description
                };
                reportProject.Add(project);
            }
            var report = new HourReportModel
            {
                TotalHours = totalHours,
                Projects = reportProject
            };
            return report;
        }
    }
}