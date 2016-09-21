using System;
using System.Diagnostics.Contracts;
using Moq;
using TechTalk.SpecFlow;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Reports;
using VinculacionBackend.Services;
using VinculacionBackend.Models;
using TechTalk.SpecFlow.Assist;
using VinculacionBackend.Data.Entities;
using System.Linq;
using System.Collections.Generic;

namespace VinculacionBackend.Specs.Features.Reports
{
    [Binding]
    public class ProjectFinalReportSteps
    {
        private readonly Mock<IProjectRepository> _projectRepositoryMock;
        private readonly Mock<IStudentRepository> _studentRepositoryMock;
        private readonly Mock<ISectionRepository> _sectionRepositoryMock;
        private readonly Mock<ISectionProjectRepository> _sectionProjectRepositoryMock;
        private ProjectFinalReport _projectFinalReport;
        private ProjectFinalReportModel _model;
        private IEnumerable<User> _students;
        public long projectId;
        public long sectionId;
        public long sectionprojectId;
        public int fieldhours;
        public int calification;
        public int beneficiariesQuantities;
        public string beneficiaieGroup;

        public ProjectFinalReportSteps()
        {
            _projectRepositoryMock = new Mock<IProjectRepository>();
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _sectionRepositoryMock = new Mock<ISectionRepository>();
            _sectionProjectRepositoryMock = new Mock<ISectionProjectRepository>();
            _projectFinalReport=new ProjectFinalReport(_projectRepositoryMock.Object,_sectionRepositoryMock.Object,_studentRepositoryMock.Object
                ,new TextDocumentServices(new TextDocument()), new DownloadbleFile(),_sectionProjectRepositoryMock.Object);
        }


        [Given(@"I have a ProjectId (.*)")]
        public void GivenIHaveAProjectId(int p0)
        {
            projectId = p0;
        }

        [Given(@"I have a SectionId (.*)")]
        public void GivenIHaveASectionId(int p0)
        {
            sectionId = p0;
        }


        [Given(@"I have a SectionProjectId (.*)")]
        public void GivenIHaveASectionProjectId(int p0)
        {
            sectionprojectId = p0;
        }
        
        [Given(@"FieldHours (.*)")]
        public void GivenFieldHours(int p0)
        {
            fieldhours = p0;
        }
        
        [Given(@"Calification (.*)")]
        public void GivenCalification(int p0)
        {
            calification = p0;
        }
        
        [Given(@"BeneficiariesQuantity (.*)")]
        public void GivenBeneficiariesQuantity(int p0)
        {
            beneficiariesQuantities = p0;
        }

        [Given(@"BeneficiarieGroups ""(.*)""")]
        public void GivenBeneficiarieGroups(string p0)
        {
            beneficiaieGroup = p0;
        }

        [Given(@"The Project with Id (.*) is")]
        public void GivenTheProjectWithIdIs(int p0, Table table)
        {
            var project = table.CreateSet<Project>().ToList()[0];
            _projectRepositoryMock.Setup(repository => repository.Get(p0)).Returns(project);
        }

        [Given(@"The Section belonging to Project with id (.*) is")]
        public void GivenTheSectionBelongingToProjectWithIdIs(int p0, Table table)
        {
            var section = new Section
            {
                Id = Int32.Parse(table.Rows[0]["Id"]),
                Code = table.Rows[0]["Code"],
                User = new User { Name = table.Rows[0]["ProfessorName"] }
            };
            _sectionRepositoryMock.Setup(repository => repository.Get(sectionId)).Returns(section);
        }

        [Given(@"The students in Section (.*) Are")]
        public void GivenTheStudentsInSectionAre(int p0, Table table)
        {
            _students = table.Rows.Select( row => new User{
                Id = Int32.Parse(row["Id"]),
                AccountId = row["AccountId"],
                Name = row["Name"],
                Major = new Major { Name = row["Major"]}
            });
            _sectionRepositoryMock.Setup(repository => repository.GetSectionStudents(p0)).Returns(_students.AsQueryable());
        }

        [Given(@"The Students Majors is ""(.*)""")]
        public void GivenTheStudentsMajorsIs(string p0)
        {
            _studentRepositoryMock.Setup(repository => repository.GetStudentMajors(_students.ToList())).Returns(p0);
        }

        [Given(@"The students hours for Project (.*) are")]
        public void GivenTheStudentsHoursForProjectAre(int p0, Table table)
        {
            var studentsHour = new Dictionary<User, int>();
            foreach(var row in table.Rows)
            {
                var student = new User { AccountId = row["AccountId"], Name = row["Name"] };
                studentsHour[student] = Int32.Parse(row["Hours"]);
            }
            _studentRepositoryMock.Setup(repository => repository.GetStudentsHoursByProject(sectionId,p0)).Returns(studentsHour);
        }


        [When(@"I execute GenerateFinalReportModel")]
        public void WhenIExecuteGenerateFinalReportModel()
        {
            _model = _projectFinalReport.GenerateFinalReportModel(projectId,sectionId,sectionprojectId, fieldhours, calification, beneficiariesQuantities, beneficiaieGroup);
        }

        [Given(@"The SectionProject with id (.*) is")]
        public void GivenTheSectionProjectWithIdIs(int p0, Table table)
        {
            var sectionProject = table.CreateInstance<SectionProject>();
            _sectionProjectRepositoryMock.Setup(repository => repository.Get(p0)).Returns(sectionProject);
        }


        [Then(@"The Final Report Model Project Should Be")]
        public void ThenTheFinalReportModelProjectShouldBe(Table table)
        {
            table.CompareToInstance(_model);
        }


        [Then(@"The final report model Section should be")]
        public void ThenTheFinalReportModelSectionShouldBe(Table table)
        {
            table.CompareToInstance(_model);
        }
        
        [Then(@"The final report model StudentsInSection should be")]
        public void ThenTheFinalReportModelStudentsInSectionShouldBe(Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"The final report model MajorsOfString should be ""(.*)""")]
        public void ThenTheFinalReportModelMajorsOfStringShouldBe(string p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"The final report model StudentsHours should be")]
        public void ThenTheFinalReportModelStudentsHoursShouldBe(Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"The final report model ProfessorName should be ""(.*)""")]
        public void ThenTheFinalReportModelProfessorNameShouldBe(string p0, Table table)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
