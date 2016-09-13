namespace VinculacionBackend.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApproveInSecProject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SectionProjects", "IsApproved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SectionProjects", "IsApproved");
        }
    }
}
