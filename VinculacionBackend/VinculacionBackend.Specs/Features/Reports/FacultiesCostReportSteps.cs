using System;
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
        private FacultiesServices _facultiesServices;
        private readonly Mock<IFacultyRepository> _facultyRepositoryMock;
        private readonly Mock<IMajorRepository> _majorRepositoryMock;
        private readonly Mock<IProjectRepository> _projectRepositoryMock;
        private readonly Mock<IStudentRepository> _studentRepositoryMock;
        private int _year;
        private  List<FacultyCostsReportEntry> _facultiesCostReport;
        public FacultiesCostReportSteps()
        {
            _facultyRepositoryMock = new Mock<IFacultyRepository>();
            _majorRepositoryMock = new Mock<IMajorRepository>();
            _projectRepositoryMock = new Mock<IProjectRepository>();
            _studentRepositoryMock = new Mock<IStudentRepository>();
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
    }
}
