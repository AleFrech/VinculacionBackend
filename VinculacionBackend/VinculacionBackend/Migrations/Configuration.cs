using VinculacionBackend.Entities;
using VinculacionBackend.Enums;

namespace VinculacionBackend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<VinculacionBackend.Database.VinculacionContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            SetSqlGenerator("MySql.Data.MySqlClient", new MySql.Data.Entity.MySqlMigrationSqlGenerator());
        }

        protected override void Seed(VinculacionBackend.Database.VinculacionContext context)
        {
            var period = new Period {Number = 1, Year = 2016};
            var clas = new Class {Name = "ANAL. Y DIS. DE SISTEMAS I"};
            var user = new User {IdNumber = "21111111",Email = "carlos.castroy@unitec.edu",Name = "Carlos Castro" ,Status = Status.Verified,Major = null,Campus = "San Pedro Sula",CreationDate = DateTime.Now,ModificationDate = DateTime.Now,Password = "1234"};
            var professorRole = new Role {Name = "Professor"};
            var section = new Section {Code = "INF405", Period = period, Class = clas, User = user};
            var proyect = new Proyect
            {
                Name = "Proyecto de Vinculacion Unitec",
                Description = "Programa para el registro de horas de vinculacion a estudiantes de Unitec sps"
            };

            context.Users.AddOrUpdate(
                x=>x.Id,
                user
                );
            context.Majors.AddOrUpdate(
                x=>x.Id,
                new Major { MajorId = "I - 01", Name  ="INGENIERÍA EN SISTEMAS COMPUTACIONALES" },
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
                x=>x.Id,
                new MajorUser {User=user,Major = null}
                );

            context.Roles.AddOrUpdate(
                x => x.Id,
                new Role { Name = "Student" },
                professorRole,
                new Role { Name = "Admin" }
                );
            context.UserRoleRels.AddOrUpdate(
                x=>x.Id,
                new UserRole {User=user,Role=professorRole}
                );


            context.Classes.AddOrUpdate(
                x=>x.Id,
                clas
                );

            context.Periods.AddOrUpdate(
              x =>x.Id,
              period
                );

            context.Proyects.AddOrUpdate(
             x => x.Id,
             proyect
                );

            context.Sections.AddOrUpdate(
                x=>x.Id,
                section
                );

            context.SectionUserRels.AddOrUpdate(
                x=>x.Id,
                new SectionUser {User = user,Section = section}
                );
            context.SectionProyectsRels.AddOrUpdate(
                x=>x.Id,
                new SectionProyect {Section = section,Proyect = proyect}
                );

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
