using System.Collections.Generic;
using System.Linq;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Exceptions;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;

namespace VinculacionBackend.Services
{
    public class HoursServices : IHoursServices
    {
        private readonly IHourRepository _hourRepository;
        private readonly IUserRepository _userRepository;

        public HoursServices(IHourRepository hourRepository, IUserRepository userRepository)
        {
            this._hourRepository = hourRepository;
            _userRepository = userRepository;
        }

        public Hour Add(HourEntryModel hourModel,string professorUser)
        {
            var isAdmin = _userRepository.isAdmin(professorUser);
            var hour =_hourRepository.InsertHourFromModel(hourModel.AccountId, hourModel.SectionId, hourModel.ProjectId, hourModel.Hour,professorUser, isAdmin);
            _hourRepository.Save();
            return hour;
        }

        public Hour Update(long hourId,HourEntryModel hourModel)
        {
            var hour = _hourRepository.Get(hourId);
            if (hour.SectionProject.IsApproved)
                throw new HoursAlreadyApprovedException("Las Horas no se pueden modificar porque ya han sido aprobadas");
            hour.Amount = hourModel.Hour;
            _hourRepository.Update(hour);
            _hourRepository.Save();
            return hour;
        }

        public void AddMany(HourCollectionEntryModel hourModel, string name)
        {
            foreach (var studenthour in hourModel.StudentsHour)
            {
                if (studenthour.HourId == -1)
                {
                    Add(
                        new HourEntryModel
                        {
                            AccountId = studenthour.AccountId,
                            Hour = studenthour.Hour,
                            ProjectId = hourModel.ProjectId,
                            SectionId = hourModel.SectionId
                        }
                        , name);
                }
                else
                {
                    Update(studenthour.HourId, new HourEntryModel
                    {
                        AccountId = studenthour.AccountId,
                        Hour = studenthour.Hour,
                        ProjectId = hourModel.ProjectId,
                        SectionId = hourModel.SectionId
                    });
                }
            }
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