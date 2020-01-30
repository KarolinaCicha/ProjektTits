namespace ProjektTitsOI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dodanieklucza : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Gospodarstwo", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Gospodarstwo", "UserId");
        }
    }
}
