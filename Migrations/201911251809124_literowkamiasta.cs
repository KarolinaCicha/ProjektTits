namespace ProjektTitsOI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class literowkamiasta : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Miasto", "Longitude", c => c.Double(nullable: false));
            DropColumn("dbo.Miasto", "Longtitude");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Miasto", "Longtitude", c => c.Double(nullable: false));
            DropColumn("dbo.Miasto", "Longitude");
        }
    }
}
