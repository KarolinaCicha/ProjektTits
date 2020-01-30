namespace ProjektTitsOI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanieNazwyMiastodoGospodarstwa : DbMigration
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
