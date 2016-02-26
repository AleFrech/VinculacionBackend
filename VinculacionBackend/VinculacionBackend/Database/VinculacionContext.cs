using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VinculacionBackend.Entities;

namespace VinculacionBackend.Database
{
    public class VinculacionContext : DbContext
    {
        public VinculacionContext() : base(ConnectionString.Get())
        {

        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Major> Majors { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User_Role> User_Role_Rels{get; set; }
        public DbSet<Major_User> Major_Users_Rels { get; set; }
    }

    public static class ConnectionString
    {
        public static string Get()
        {
            var Environment = ConfigurationManager.AppSettings["Environment"];
            return string.Format("name={0}", Environment);
        }
    }

}