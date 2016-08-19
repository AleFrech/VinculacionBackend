using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using VinculacionBackend.Cache;
using VinculacionBackend.Data;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Security;
using VinculacionBackend.Services;
using VinculacionBackend.Data.Repositories;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Reports;

namespace VinculacionBackend
{
    public static class AutofacConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            RegisterServices(builder);
            RegisterRepositories(builder);

            var container = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver =
            new AutofacWebApiDependencyResolver(container);
        }

        private static void RegisterRepositories(ContainerBuilder builder)

        {
            builder.RegisterType<FacultyRepository>().As<IFacultyRepository>().InstancePerRequest();
            builder.RegisterType<StudentRepository>().As<IStudentRepository>().InstancePerRequest();
            builder.RegisterType<MajorRepository>().As<IMajorRepository>().InstancePerRequest();
            builder.RegisterType<HourRepository>().As<IHourRepository>().InstancePerRequest();
            builder.RegisterType<ProjectRepository>().As<IProjectRepository>().InstancePerRequest();
            builder.RegisterType<ProfessorRepository>().As<IProfessorRepository>().InstancePerRequest();
            builder.RegisterType<ClassRepository>().As<IClassRepository>().InstancePerRequest();
            builder.RegisterType<PeriodRepository>().As<IPeriodRepository>().InstancePerRequest();
            builder.RegisterType<SectionRepository>().As<ISectionRepository>().InstancePerRequest();
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerRequest();
            builder.RegisterType<SectionProjectRepository>().As<ISectionProjectRepository>().InstancePerRequest();


        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<ProfessorsServices>().As<IProfessorsServices>().InstancePerRequest();
            builder.RegisterType<StudentsServices>().As<IStudentsServices>().InstancePerRequest();
            builder.RegisterType<PeriodsServices>().As<IPeriodsServices>().InstancePerRequest();
            builder.RegisterType<MajorsServices>().As<IMajorsServices>().InstancePerRequest();
            builder.RegisterType<HoursServices>().As<IHoursServices>().InstancePerRequest();
            builder.RegisterType<UsersServices>().As<IUsersServices>().InstancePerRequest();
            builder.RegisterType<ClassesServices>().As<IClassesServices>().InstancePerRequest();
            builder.RegisterType<ProjectServices>().As<IProjectServices>().InstancePerRequest();
            builder.RegisterType<TextDocument>().As<ITextDoucment>().InstancePerRequest();
            builder.RegisterType<TextDocumentServices>().As<ITextDocumentServices>().InstancePerRequest();
            builder.RegisterType<Email>().As<IEmail>().InstancePerRequest();
            builder.RegisterType<MemoryCacher>().As<IMemoryCacher>().InstancePerRequest();
            builder.RegisterType<Encryption>().As<IEncryption>().InstancePerRequest();
            builder.RegisterType<DownloadbleFile>().As<IDownloadbleFile>().InstancePerRequest();
            builder.RegisterType<SectionsServices>().As<ISectionsServices>().InstancePerRequest();
            builder.RegisterType<FacultiesServices>().As<IFacultiesServices>().InstancePerRequest();
            builder.RegisterType<SectionProjectServices>().As<ISectionProjectServices>().InstancePerRequest();
            builder.RegisterType<ReportsServices>().As<ISheetsReportsServices>().InstancePerRequest();


        }
    }

   

}