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
    public class StudentCountReportSteps
    {
        private StudentsServices _studentServices;
        private readonly Mock<IStudentRepository> _studentRepositoryMock;
        private readonly Mock<IMajorsServices> _majorServicesMock;
        private readonly Mock<IEncryption> _encryptionMock;
        private readonly Mock<ITextDocumentServices> _textDocumentServicesMock;
        private readonly Mock<IHourRepository> _hourRepositoryMock;
        private List<StudentReportModel> _studentCountReport;
        private int _year;

        public StudentCountReportSteps()
        {
            _majorServicesMock = new Mock<IMajorsServices>();
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _encryptionMock = new Mock<IEncryption>();
            _textDocumentServicesMock = new Mock<ITextDocumentServices>();
            _hourRepositoryMock = new Mock<IHourRepository>();
            _studentServices = new StudentsServices(_studentRepositoryMock.Object, _encryptionMock.Object, _majorServicesMock.Object, 
                                                    _textDocumentServicesMock.Object, _hourRepositoryMock.Object);
        }

        [Given(@"the amount of students for Class '(.*)' and year (.*) is")]
        public void GivenTheAmountOfStudentsForClassAndYearIs(string clase, int year, Table table)
        {
            var studentCount = table.CreateSet<StudentCountModel>().ToList();
            _studentRepositoryMock.Setup(l => l.GetStudentCount(clase, year)).Returns(studentCount.First());
        }

        [Given(@"the amount of students for Faculty (.*) and year (.*) is")]
        public void GivenTheAmountOfStudentsForFacultyAndYearIs(int faculty, int year, Table table)
        {
            var studentCount = table.CreateSet<StudentCountModel>().ToList();
            _studentRepositoryMock.Setup(l => l.GetStudentByFacultyCount(faculty, year)).Returns(studentCount.First());
        }

        [Given(@"the current year is (.*)")]
        public void GivenTheCurrentYearIs(int year)
        {
            _year = year;
        }


        [When(@"I execute the student count report")]
        public void WhenIExecuteTheStudentCountReport()
        {
            _studentCountReport = _studentServices.CreateStudentReport(_year);
        }
        
        [Then(@"the student count report should be")]
        public void ThenTheStudentCountReportShouldBe(Table table)
        {
            table.CompareToSet(_studentCountReport.AsEnumerable());
        }
    }
}
