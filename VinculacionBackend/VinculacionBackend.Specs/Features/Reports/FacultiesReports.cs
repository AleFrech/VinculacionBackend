﻿using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Data.Models;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;
using VinculacionBackend.Services;

namespace VinculacionBackend.Specs.Features.Reports
{
    [Binding]
    public class FacultiesCostReportSteps
    {
        private readonly FacultiesServices _facultiesServices;
        private readonly ProjectServices _projectServices;
        private readonly Mock<IClassRepository> _classRepositoryMock;
        private readonly Mock<IFacultyRepository> _facultyRepositoryMock;
        private readonly Mock<IMajorRepository> _majorRepositoryMock;
        private readonly Mock<IProjectRepository> _projectRepositoryMock;
        private readonly Mock<IStudentRepository> _studentRepositoryMock;
        private readonly Mock<IPeriodRepository> _periodRepositoryMock;
        private int _year;
        private List<ProjectByMajorEntryModel> _projectsByMajorReport;
        private List<FacultyHoursReportEntryModel> _hoursReport;
        private  List<FacultyCostsReportEntry> _facultiesCostReport;
        public FacultiesCostReportSteps()
        {
            _facultyRepositoryMock = new Mock<IFacultyRepository>();
            _majorRepositoryMock = new Mock<IMajorRepository>();
            _projectRepositoryMock = new Mock<IProjectRepository>();
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _classRepositoryMock = new Mock<IClassRepository>();
            _periodRepositoryMock = new Mock<IPeriodRepository>();
            _projectServices = new ProjectServices(_projectRepositoryMock.Object, _majorRepositoryMock.Object, _classRepositoryMock.Object, _periodRepositoryMock.Object);
            _facultiesServices = new FacultiesServices(_facultyRepositoryMock.Object,_majorRepositoryMock.Object,
                                                        _projectRepositoryMock.Object,_studentRepositoryMock.Object);
        }

        [Given(@"I have this faculties")]
        public void GivenIHaveThisFaculties(Table table)
        {
            var faculties = table.CreateSet<Faculty>().AsQueryable();
            _facultyRepositoryMock.Setup(l => l.GetAll()).Returns(faculties);
        }

        [Given(@"the year is (.*)")]
        public void GivenTheYearIs(int year)
        {
            _year = year;
        }

        [Given(@"the cost for faculty (.*) for the period (.*) and year (.*) is")]
        public void GivenTheCostForFacultyForThePeriodAndYearIs(int falcultyId, int period, int year, Table table)
        {
            var facultyCosts = table.CreateSet<FacultyProjectCostModel>().ToList();
            _facultyRepositoryMock.Setup(l => l.GetFacultyCosts(falcultyId, period, year)).Returns(facultyCosts);
        }
        
        [When(@"I execute the faculties cost report")]
        public void WhenIExecuteTheFacultiesCostReport()
        {
           _facultiesCostReport = _facultiesServices.CreateFacultiesCostReport(_year);

        }
        
        [Then(@"the faculties cost report should be")]
        public void ThenTheFacultiesCostReportShouldBe(Table table)
        {
            table.CompareToSet(_facultiesCostReport.AsEnumerable());
        }

        [Given(@"the hours for faculty (.*) for the year (.*) is")]
        public void GivenTheHoursForFacultyForTheYearIs(int p0, int p1, Table table)
        {
            var hours = table.CreateSet<FacultyHoursReportModel>().ToList();
            var totalHours = hours.Sum(x => x.ProjectHours);
            _facultyRepositoryMock.Setup(x => x.GetFacultyHours(p0, p1)).Returns(totalHours);
        }

        [When(@"I execute the faculties hours report")]
        public void WhenIExecuteTheFacultiesHoursReport()
        {
            _hoursReport = _facultiesServices.CreateFacultiesHourReport(_year);
        }

        [Then(@"the faculties hour report should be")]
        public void ThenTheFacultiesHourReportShouldBe(Table table)
        {
            table.CompareToSet(_hoursReport.AsEnumerable());
        }

        [Given(@"I have this majors")]
        public void GivenIHaveThisMajors(Table table)
        {
            var majors = table.CreateSet<Major>().AsQueryable();
            _majorRepositoryMock.Setup(x => x.GetAll()).Returns(majors);
        }

        [Given(@"This is the current period")]
        public void GivenThisIsTheCurrentPeriod(Table table)
        {
            var period = table.CreateSet<Period>().ToList();
            _periodRepositoryMock.Setup(x => x.GetCurrent()).Returns(period[0]);
        }

        [Given(@"I have the majors and it has many projects")]
        public void GivenIHaveTheMajorsAndItHasManyProjects(Table table)
        {
            var majorProjectList = table.CreateSet<MajorProjectTotalmodel>().AsQueryable();
            _projectRepositoryMock.Setup(x => x.GetMajorProjectTotal(It.IsAny<Period>(), It.IsAny<string>()))
                .Returns((Period period, string majorId) => majorProjectList.Where(x => x.MajorId == majorId).Select(x => new MajorProjectTotalmodel
                {
                    MajorId = x.MajorId,
                    Major = x.Major,
                    Total = x.Total
                }).ToList());
        }


        [When(@"I execute the projects by major report")]
        public void WhenIExecuteTheProjectsByMajorReport()
        {
            _projectsByMajorReport = _projectServices.CreateProjectsByMajor();
        }

        [Then(@"I have the projects")]
        public void ThenIHaveTheProjects(Table table)
        {
            table.CompareToSet(_projectsByMajorReport.AsEnumerable());
        }
        
    }
}
