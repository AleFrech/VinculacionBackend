using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Data.Repositories;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Services;

namespace VinculacionBackend.Specs.Features.Reports
{
    [Binding]
    public class HourNumberReportSteps
    {
        private StudentsServices _studentServices;
        private readonly Mock<IStudentRepository> _studentRepositoryMock;
        private readonly Mock<IMajorsServices> _majorServicesMock;
        private readonly Mock<IEncryption> _encryptionMock;
        private readonly Mock<ITextDocumentServices> _textDocumentServicesMock;
        private readonly Mock<IHourRepository> _hourRepositoryMock;
        private List<StudentReportModel> _hoursCountReport;
        private int _year;

        public HourNumberReportSteps()
        {
            _majorServicesMock = new Mock<IMajorsServices>();
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _encryptionMock = new Mock<IEncryption>();
            _textDocumentServicesMock = new Mock<ITextDocumentServices>();
            _hourRepositoryMock = new Mock<IHourRepository>();
            _studentServices = new StudentsServices(_studentRepositoryMock.Object, _encryptionMock.Object, _majorServicesMock.Object,
                                                    _textDocumentServicesMock.Object, _hourRepositoryMock.Object);
        }

        [Given(@"the amount of hours for Class '(.*)' and year (.*) is")]
        public void GivenTheAmountOfHoursForClassAndYearIs(string @class, int year, Table table)
        {
            var studentCount = table.CreateSet<StudentCountModel>().ToList();
            _studentRepositoryMock.Setup(l => l.GetHoursCount(@class, year)).Returns(studentCount.First());
        }
        
        [Given(@"the amount of hours for Faculty (.*) and year (.*) is")]
        public void GivenTheAmountOfHoursForFacultyAndYearIs(int faculty, int year, Table table)
        {
            var studentCount = table.CreateSet<StudentCountModel>().ToList();
            _studentRepositoryMock.Setup(l => l.GetHoursByFacultyCount(faculty, year)).Returns(studentCount.First());
        }
        
        [Given(@"the hours year is (.*)")]
        public void GivenTheHoursYearIs(int year)
        {
            _year = year;
        }
        
        [When(@"I execute the hour number report")]
        public void WhenIExecuteTheHourNumberReport()
        {
            _hoursCountReport = _studentServices.CreateHourNumberReport(_year);
        }
        
        [Then(@"the hour number report should be")]
        public void ThenTheHourNumberReportShouldBe(Table table)
        {
            table.CompareToSet(_hoursCountReport.AsEnumerable());
        }
    }
}
