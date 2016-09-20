using System.Collections.Generic;
using System.Linq;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Models;
using VinculacionBackend.Data.Repositories;

namespace VinculacionBackend.Data.Interfaces
{
    public interface IProjectRepository : IRepository<Project>
    {
        IQueryable<User> GetProjectStudents(long projectId);
        void Insert(Project ent, List<string> majorIds);
        void AssignToSection(long projectId, long sectionId);
        SectionProject RemoveFromSection(long projectId, long sectionId);
        IQueryable<Project> GetAllStudent(long userId);
        IQueryable<Project> GetAllProfessor(long userId);
        Section GetSection(long projectId);
        IQueryable<Project> GetByMajor(string majorId);
        List<MajorProjectTotalmodel> GetMajorProjectTotal(Period currentPeriod, string majorId);
        IQueryable<Project> GetProjectsByClass(long classId);
        IQueryable<User> GetProfessorsByProject(long projectId);
        Period GetPeriodByProject(long projectId);
        string GetClass(long v);
        string GetMajors(List<Major> majors);
        string GetProfessor(long id);
        string GetTotalHours(long id);
        double GetTotalCostByProject(long projectId);
        IQueryable<PeriodReportModel> GetByYearAndPeriod(int year, int period);
        IQueryable<Project> GetProjectsBySection(long sectionId);
        SectionProject GetSectionProject(long projectId, long sectionId);
        string GetNextProjectCode(Period currentPeriod);
        void Update(Project ent, List<string> majorIds);
    }
}
