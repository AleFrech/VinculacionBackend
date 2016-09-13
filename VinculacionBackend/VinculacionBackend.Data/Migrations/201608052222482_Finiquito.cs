namespace VinculacionBackend.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Finiquito : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Finiquiteado", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Finiquiteado");
        }
    }
}
