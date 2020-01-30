namespace ProjektTitsOI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanieModeli : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Gospodarstwo",
                c => new
                    {
                        GospodarstwoId = c.Int(nullable: false, identity: true),
                        NazwaGospodarstwa = c.String(),
                        LiczbaOsob = c.Int(nullable: false),
                        LiczbaPaneli = c.Int(nullable: false),
                        MiastoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GospodarstwoId);
            
            CreateTable(
                "dbo.Pomiar",
                c => new
                    {
                        PomiarId = c.Int(nullable: false, identity: true),
                        Data = c.DateTime(nullable: false),
                        EnergiaWyprodukowana = c.Int(nullable: false),
                        EnergiaZuzyta = c.Int(nullable: false),
                        Dlugoscdnia = c.Double(nullable: false),
                        Pogoda = c.String(),
                        GospodarstwoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PomiarId)
                .ForeignKey("dbo.Gospodarstwo", t => t.GospodarstwoId, cascadeDelete: true)
                .Index(t => t.GospodarstwoId);
            
            CreateTable(
                "dbo.Miasto",
                c => new
                    {
                        MiastoId = c.Int(nullable: false, identity: true),
                        Nazwa = c.String(nullable: false),
                        Latitude = c.Double(nullable: false),
                        Longtitude = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.MiastoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pomiar", "GospodarstwoId", "dbo.Gospodarstwo");
            DropIndex("dbo.Pomiar", new[] { "GospodarstwoId" });
            DropTable("dbo.Miasto");
            DropTable("dbo.Pomiar");
            DropTable("dbo.Gospodarstwo");
        }
    }
}
