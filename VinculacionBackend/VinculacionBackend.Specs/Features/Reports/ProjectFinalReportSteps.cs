using System;
using System.Diagnostics.Contracts;
using Moq;
using TechTalk.SpecFlow;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Reports;
using VinculacionBackend.Services;

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
        public long projectId;
        public long sectionId;
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
        
        [Given(@"I have a SectionProjectId (.*)")]
        public void GivenIHaveASectionProjectId(int p0)
        {
            sectionId = p0;
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
        
        [When(@"I execute GenerateFinalReportModel")]
        public void WhenIExecuteGenerateFinalReportModel()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"The final report model project should be")]
        public void ThenTheFinalReportModelProjectShouldBe(Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"The final report model Section should be")]
        public void ThenTheFinalReportModelSectionShouldBe(Table table)
        {
            ScenarioContext.Current.Pending();
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
