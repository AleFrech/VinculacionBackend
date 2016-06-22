namespace VinculacionBackend.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropBenficiariesGroupAndQuantity : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Projects", "BeneficiariesQuantity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Projects", "BeneficiariesQuantity", c => c.Int(nullable: false));
        }
    }
}
