namespace ProjektTitsOI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dodanieklucza2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Gospodarstwo", name: "UserId", newName: "ApplicationUser_Id");
            RenameIndex(table: "dbo.Gospodarstwo", name: "IX_UserId", newName: "IX_ApplicationUser_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Gospodarstwo", name: "IX_ApplicationUser_Id", newName: "IX_UserId");
            RenameColumn(table: "dbo.Gospodarstwo", name: "ApplicationUser_Id", newName: "UserId");
        }
    }
}
