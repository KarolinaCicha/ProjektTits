namespace ProjektTitsOI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class usunieciezmiast2 : DbMigration
    {
        public override void Up()
        {
            Sql("Delete from miasto");
        }
        
        public override void Down()
        {
        }
    }
}
