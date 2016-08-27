namespace VinculacionBackend.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SectionProjectRestructure : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SectionProjects", "Description", c => c.String(unicode: false));
            AddColumn("dbo.SectionProjects", "Cost", c => c.Double(nullable: false));
            DropColumn("dbo.Projects", "Cost");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Projects", "Cost", c => c.Double(nullable: false));
            DropColumn("dbo.SectionProjects", "Cost");
            DropColumn("dbo.SectionProjects", "Description");
        }
    }
}
