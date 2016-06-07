namespace VinculacionBackend.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Classes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Code = c.String(unicode: false),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Faculties",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Hours",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Amount = c.Int(nullable: false),
                        SectionProject_Id = c.Long(),
                        User_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SectionProjects", t => t.SectionProject_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.SectionProject_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.SectionProjects",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Project_Id = c.Long(),
                        Section_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .ForeignKey("dbo.Sections", t => t.Section_Id)
                .Index(t => t.Project_Id)
                .Index(t => t.Section_Id);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProjectId = c.String(unicode: false),
                        Name = c.String(unicode: false),
                        Description = c.String(unicode: false),
                        Cost = c.Double(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        BeneficiarieOrganization = c.String(unicode: false),
                        BeneficiarieGroups = c.String(unicode: false),
                        BeneficiariesQuantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sections",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Code = c.String(unicode: false),
                        Class_Id = c.Long(),
                        Period_Id = c.Long(),
                        User_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Classes", t => t.Class_Id)
                .ForeignKey("dbo.Periods", t => t.Period_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Class_Id)
                .Index(t => t.Period_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Periods",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        FromDate = c.String(unicode: false),
                        ToDate = c.String(unicode: false),
                        IsCurrent = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AccountId = c.String(unicode: false),
                        Name = c.String(unicode: false),
                        Password = c.String(unicode: false),
                        Campus = c.String(unicode: false),
                        Email = c.String(unicode: false),
                        CreationDate = c.DateTime(nullable: false, precision: 0),
                        ModificationDate = c.DateTime(nullable: false, precision: 0),
                        Status = c.Int(nullable: false),
                        Major_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Majors", t => t.Major_Id)
                .Index(t => t.Major_Id);
            
            CreateTable(
                "dbo.Majors",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MajorId = c.String(unicode: false),
                        Name = c.String(unicode: false),
                        Faculty_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Faculties", t => t.Faculty_Id)
                .Index(t => t.Faculty_Id);
            
            CreateTable(
                "dbo.MajorUsers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Major_Id = c.Long(),
                        User_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Majors", t => t.Major_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Major_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.ProjectMajors",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Major_Id = c.Long(),
                        Project_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Majors", t => t.Major_Id)
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .Index(t => t.Major_Id)
                .Index(t => t.Project_Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SectionUsers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Section_Id = c.Long(),
                        User_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sections", t => t.Section_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Section_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Role_Id = c.Long(),
                        User_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.Role_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Role_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRoles", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.SectionUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.SectionUsers", "Section_Id", "dbo.Sections");
            DropForeignKey("dbo.ProjectMajors", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.ProjectMajors", "Major_Id", "dbo.Majors");
            DropForeignKey("dbo.MajorUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.MajorUsers", "Major_Id", "dbo.Majors");
            DropForeignKey("dbo.Hours", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Hours", "SectionProject_Id", "dbo.SectionProjects");
            DropForeignKey("dbo.SectionProjects", "Section_Id", "dbo.Sections");
            DropForeignKey("dbo.Sections", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "Major_Id", "dbo.Majors");
            DropForeignKey("dbo.Majors", "Faculty_Id", "dbo.Faculties");
            DropForeignKey("dbo.Sections", "Period_Id", "dbo.Periods");
            DropForeignKey("dbo.Sections", "Class_Id", "dbo.Classes");
            DropForeignKey("dbo.SectionProjects", "Project_Id", "dbo.Projects");
            DropIndex("dbo.UserRoles", new[] { "User_Id" });
            DropIndex("dbo.UserRoles", new[] { "Role_Id" });
            DropIndex("dbo.SectionUsers", new[] { "User_Id" });
            DropIndex("dbo.SectionUsers", new[] { "Section_Id" });
            DropIndex("dbo.ProjectMajors", new[] { "Project_Id" });
            DropIndex("dbo.ProjectMajors", new[] { "Major_Id" });
            DropIndex("dbo.MajorUsers", new[] { "User_Id" });
            DropIndex("dbo.MajorUsers", new[] { "Major_Id" });
            DropIndex("dbo.Majors", new[] { "Faculty_Id" });
            DropIndex("dbo.Users", new[] { "Major_Id" });
            DropIndex("dbo.Sections", new[] { "User_Id" });
            DropIndex("dbo.Sections", new[] { "Period_Id" });
            DropIndex("dbo.Sections", new[] { "Class_Id" });
            DropIndex("dbo.SectionProjects", new[] { "Section_Id" });
            DropIndex("dbo.SectionProjects", new[] { "Project_Id" });
            DropIndex("dbo.Hours", new[] { "User_Id" });
            DropIndex("dbo.Hours", new[] { "SectionProject_Id" });
            DropTable("dbo.UserRoles");
            DropTable("dbo.SectionUsers");
            DropTable("dbo.Roles");
            DropTable("dbo.ProjectMajors");
            DropTable("dbo.MajorUsers");
            DropTable("dbo.Majors");
            DropTable("dbo.Users");
            DropTable("dbo.Periods");
            DropTable("dbo.Sections");
            DropTable("dbo.Projects");
            DropTable("dbo.SectionProjects");
            DropTable("dbo.Hours");
            DropTable("dbo.Faculties");
            DropTable("dbo.Classes");
        }
    }
}
