using System.Configuration;
using System.Data.Entity;
using VinculacionBackend.Data.Entities;

namespace VinculacionBackend.Data.Database
{
    public class VinculacionContext:DbContext
    {
        public VinculacionContext() : base(ConnectionString.Get())
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Major> Majors { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<SectionProject> SectionProjectsRels { get; set; }
        public DbSet<SectionUser> SectionUserRels { get; set; }
        public DbSet<Hour> Hours { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoleRels { get; set; }
        public DbSet<MajorUser> MajorUsersRels { get; set; }
        public DbSet<ProjectMajor> ProjectMajorRels { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
    }

    public static class ConnectionString
    {
        public static string Get()
        {
#if DEBUG
            var environment = ConfigurationManager.AppSettings["DevEnvironment"];
            return string.Format((string) "name={0}", (object) environment);
#else
            var environment = ConfigurationManager.AppSettings["ProductionEnvironment"];
            return string.Format((string)"name={0}", (object)environment);
#endif

        }
    }

}