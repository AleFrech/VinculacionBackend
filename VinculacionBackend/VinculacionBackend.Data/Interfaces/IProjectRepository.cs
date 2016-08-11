using System.Collections.Generic;
using System.Linq;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Models;

namespace VinculacionBackend.Data.Interfaces
{
    public interface IProjectRepository : IRepository<Project>
    {
        IQueryable<User> GetProjectStudents(long projectId);
        void Insert(Project ent, List<string> majorIds, List<long> sectionIds);
        void AssignToSection(long projectId, long sectionId);
        SectionProject RemoveFromSection(long projectId, long sectionId);
        IQueryable<Project> GetAllStudent(long userId);
        IQueryable<Project> GetAllProfessor(long userId);
        Section GetSection(Project project);
        IQueryable<Project> GetByMajor(string majorId);
        List<MajorProjectTotalmodel> GetMajorProjectTotal(Period currentPeriod, string majorId);
        IQueryable<Project> GetProjectsByClass(long classId);
        IQueryable<User> GetProfessorsByProject(long projectId);
        Period GetPeriodByProject(long projectId);
        string GetClass(long v);
        string GetMajors(List<Major> majors);
        string GetProfessor(long id);
        string GetTotalHours(long id);
        IQueryable<Project> GetByYearAndPeriod(int year, int period);
        IQueryable<Project> GetProjectsBySection(long sectionId);
    }
}
