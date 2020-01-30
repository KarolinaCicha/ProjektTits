namespace ProjektTitsOI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class usunieciezmiast : DbMigration
    {
        public override void Up()
        {
           Sql("DELETE FROM Miasto");
        }
        
        public override void Down()
        {
        }
    }
}
