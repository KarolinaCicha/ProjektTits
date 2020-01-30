namespace ProjektTitsOI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanieKluczyObcych : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Gospodarstwo", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Gospodarstwo", "UserId");
            AddForeignKey("dbo.Gospodarstwo", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Gospodarstwo", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Gospodarstwo", new[] { "UserId" });
            DropColumn("dbo.Gospodarstwo", "UserId");
        }
    }
}
