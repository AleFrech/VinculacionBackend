namespace VinculacionBackend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedClassInSection : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sections", "Class_Id", c => c.Long());
            CreateIndex("dbo.Sections", "Class_Id");
            AddForeignKey("dbo.Sections", "Class_Id", "dbo.Classes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sections", "Class_Id", "dbo.Classes");
            DropIndex("dbo.Sections", new[] { "Class_Id" });
            DropColumn("dbo.Sections", "Class_Id");
        }
    }
}
