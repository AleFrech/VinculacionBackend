using VinculacionBackend.Data.Database;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Enums;

namespace VinculacionBackend.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<VinculacionContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            SetSqlGenerator("MySql.Data.MySqlClient", new MySql.Data.Entity.MySqlMigrationSqlGenerator());
        }

        protected override void Seed(VinculacionContext context)
        {
            var period = new Period { Number = 1, Year = 2016 };
            var clas = new Class { Name = "ANAL. Y DIS. DE SISTEMAS I" };
            var user = new User { AccountId = "21111111", Email = "carlos.castroy@unitec.edu", Name = "Carlos Castro", Status = Status.Verified, Major = null, Campus = "San Pedro Sula", CreationDate = DateTime.Now, ModificationDate = DateTime.Now, Password = "1234" };
            var professorRole = new Role { Name = "Professor" };
            var studentRole = new Role { Name = "Student" };
            var major1 = new Major { MajorId = "I - 01", Name = "INGENIERÍA EN SISTEMAS COMPUTACIONALES" };
            var user2 = new User
            {
                AccountId = "21241095",
                Campus = "SPS",
                CreationDate = DateTime.Now,
                Email = "efherreram@unitec.edu",
                Name = "Edwin Herrear",
                ModificationDate = DateTime.Now,
                Password = "holis",
                Status = Status.Inactive,
                Major = major1
            };
            var section = new Section { Code = "INF405", Period = period, Class = clas, User = user };
            var project = new Project
            {
                Name = "Proyecto de Vinculacion Unitec",
                Description = "Programa para el registro de horas de vinculacion a estudiantes de Unitec sps"
            };

            var projectMajor = new ProjectMajor {Major = major1, Project = project};

            var sectionProject = new SectionProject { Section = section, Project = project };
            context.Users.AddOrUpdate(
                x => x.Id,
                user
                );
            context.Majors.AddOrUpdate(
                x => x.Id,
                major1,
                new Major { MajorId = "I - 02", Name = "INGENIERÍA INDUSTRIAL Y DE SISTEMAS" },
                new Major { MajorId = "I - 03", Name = "INGENIERÍA CIVIL" },
                new Major { MajorId = "I - 04", Name = "INGENIERÍA EN TELECOMUNICACIONES" },
                new Major { MajorId = "I - 05", Name = "INGENIERÍA EN MECATRÓNICANICA" },
                new Major { MajorId = "I - 06", Name = "INGENIERÍA EN INFORMÁTICA" },
                new Major { MajorId = "I - 07", Name = "INGENIERÍA EN SISTEMAS ELECTRÓNICOS" },
                new Major { MajorId = "I - 09", Name = "INGENIERÍA EN GESTIÓN LOGÍSTICA" },
                new Major { MajorId = "I - 10", Name = "INGENIERÍA EN BIOMÉDICA" },
                new Major { MajorId = "I - 11", Name = "ARQUITECTURA" },
                new Major { MajorId = "I - 12", Name = "INGENIERÍA EN ENERGÍA" },
                new Major { MajorId = "L - 02", Name = "LICENCIATURA EN ADMINISTRACIÓN INDUSTRIAL Y DE NEGOCIOS" },
                new Major { MajorId = "L - 04", Name = "LICENCIATURA EN FINANZAS" },
                new Major { MajorId = "L - 06", Name = "LICENCIATURA EN MERCADOTECNIA(PROMOCIÓN Y PUBLICIDAD)" },
                new Major { MajorId = "L - 07", Name = "LICENCIATURA EN COMUNICACIÓN Y PUBLICIDAD" },
                new Major { MajorId = "L - 08", Name = "LICENCIATURA EN MERCADOTECNIA Y NEGOCIOS INTERNACIONALES" },
                new Major { MajorId = "L - 09", Name = "LICENCIATURA EN ADMINISTRACIÓN DE EMPRESAS TURÍSTICAS" },
                new Major { MajorId = "L - 10", Name = "LICENCIATURA EN DERECHO" },
                new Major { MajorId = "L - 12", Name = "LICENCIATURA EN DISEÑO GRAFICO" },
                new Major { MajorId = "L - 13", Name = "LICENCIATURA EN RELACIONES INTERNACIONALES" },
                new Major { MajorId = "L - 14", Name = "LICENCIATURA EN PSICOLOGÍA CON ORIENTACIÓN EMPRESARIAL" },
                new Major { MajorId = "L - 15", Name = "LICENCIATURA EN CONTADURÍA PÚBLICA" },
                new Major { MajorId = "L - 16", Name = "LICENCIATURA EN ADMINISTRACIÓN DE EMPRESAS(CEUTEC)" },
                new Major { MajorId = "T - 03", Name = "TÉCNICO UNIVERSITARIO EN MERCADOTECNIA Y VENTAS(CEUTEC)" },
                new Major { MajorId = "T - 05", Name = "TÉCNICO UNIVERSITARIO EN ADMINISTRACIÓN(CEUTEC)" },
                new Major { MajorId = "T - 07", Name = "TÉCNICO EN DESARROLLO DE SISTEMAS DE INFORMACIÓN(CEUTEC)" },
                new Major { MajorId = "T - 08", Name = "TÉCNICO UNIVERSITARIO EN DISEÑO GRÁFICO(CEUTEC)" }
                );

            context.MajorUsersRels.AddOrUpdate(
                x => x.Id,
                new MajorUser { User = user, Major = null }
                );

            context.Roles.AddOrUpdate(
                x => x.Id,
                studentRole,
                professorRole,
                new Role { Name = "Admin" }
                );
            context.UserRoleRels.AddOrUpdate(
                x => x.Id,
                new UserRole { User = user, Role = professorRole },
                new UserRole { User = user2, Role = studentRole }
                );


            context.Classes.AddOrUpdate(
                x => x.Id,
                clas
                );

            context.Periods.AddOrUpdate(
              x => x.Id,
              period
                );

            context.Projects.AddOrUpdate(
             x => x.Id,
             project
                );

            context.ProjectMajorRels.AddOrUpdate(
                x => x.Id,
                projectMajor
                );

            context.Sections.AddOrUpdate(
                x => x.Id,
                section
                );

            context.SectionUserRels.AddOrUpdate(
                x => x.Id,
                new SectionUser { User = user, Section = section }
                );
            context.SectionProjectsRels.AddOrUpdate(
                x => x.Id,
                sectionProject
                );

            context.Hours.AddOrUpdate(
                x => x.Id,
                new Hour { Amount = 5, SectionProject = sectionProject, User = user2 }
                );
        }
    }
}
