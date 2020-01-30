namespace ProjektTitsOI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsuniecieNazwyMiastaZGospodarstw2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Gospodarstwo", "MiastoNazwa");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Gospodarstwo", "MiastoNazwa", c => c.String());
        }
    }
}
