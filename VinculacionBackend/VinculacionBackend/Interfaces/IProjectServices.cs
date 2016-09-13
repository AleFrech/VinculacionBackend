using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Repositories;
using VinculacionBackend.Models;

namespace VinculacionBackend.Interfaces
{
    public interface IProjectServices
    {
        Project Find(long id);
        IQueryable<Project> All();
        int GetProjectsTotalByMajor(Major major);
        List<ProjectByMajorEntryModel> CreateProjectsByMajor();
        Project Add(ProjectModel project);
        Project Delete(long projectId);
        IQueryable<User> GetProjectStudents(long projectId);
        Project UpdateProject(long projectId, ProjectModel model);
        bool AssignSection(ProjectSectionModel model);
        bool RemoveFromSection(long projectId, long sectionId);
        IQueryable<Project> GetUserProjects(long userId, string[] roles);
        HttpResponseMessage GetFinalReport(long projectId,long sectionId, int fieldHours, int calification, int beneficiariesQuantities, string beneficiariGroups);
        List<ProjectsByClassEntryModel> ProjectsByClass(long classId);
        IQueryable<PeriodReportModel> CreatePeriodReport(int year, int period);
        void AssignProjectsToSection(ProjectsSectionModel model);
        IQueryable<Project> GetProjectsBySection(long sectionId);
    }
}