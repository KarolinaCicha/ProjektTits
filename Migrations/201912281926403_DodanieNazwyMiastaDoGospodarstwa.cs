namespace ProjektTitsOI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanieNazwyMiastaDoGospodarstwa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Gospodarstwo", "MiastoNazwa", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Gospodarstwo", "MiastoNazwa");
        }
    }
}
