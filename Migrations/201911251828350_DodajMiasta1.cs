namespace ProjektTitsOI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodajMiasta1 : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Miasto (Nazwa,Latitude,Longitude) VALUES('Bia³ystok',53.13305556,23.16416667)");
            Sql("INSERT INTO Miasto (Nazwa,Latitude,Longitude) VALUES('Bydgoszcz',53.12333333,18.00750000)");
            Sql("INSERT INTO Miasto (Nazwa,Latitude,Longitude) VALUES('Gdañsk',54.35194444,18.64611111)");
            Sql("INSERT INTO Miasto (Nazwa,Latitude,Longitude) VALUES('Gorzów Wielkopolski',52.73666667,15.22861111)");
            Sql("INSERT INTO Miasto (Nazwa,Latitude,Longitude) VALUES('Katowice',50.25833333,19.02750000)");
            Sql("INSERT INTO Miasto (Nazwa,Latitude,Longitude) VALUES('Kielce',50.87027778,20.62750000)");
            Sql("INSERT INTO Miasto (Nazwa,Latitude,Longitude) VALUES('Kraków',50.06138889,19.93638889)");
            Sql("INSERT INTO Miasto (Nazwa,Latitude,Longitude) VALUES('Lublin',51.25000000,22.56666667)");
            Sql("INSERT INTO Miasto (Nazwa,Latitude,Longitude) VALUES('£ódŸ',51.75000000,19.46666667)");
            Sql("INSERT INTO Miasto (Nazwa,Latitude,Longitude) VALUES('Olsztyn',53.77972222,20.49388889)");
            Sql("INSERT INTO Miasto (Nazwa,Latitude,Longitude) VALUES('Opole',50.67194444,17.92527778)");
            Sql("INSERT INTO Miasto (Nazwa,Latitude,Longitude) VALUES('Poznañ',52.41666667,16.91666667)");
            Sql("INSERT INTO Miasto (Nazwa,Latitude,Longitude) VALUES('Rzeszów',50.04111111,21.99888889)");
            Sql("INSERT INTO Miasto (Nazwa,Latitude,Longitude) VALUES('Szczeciñ',53.42888889,14.55277778)");
            Sql("INSERT INTO Miasto (Nazwa,Latitude,Longitude) VALUES('Toruñ',53.01361111,18.59805556)");
            Sql("INSERT INTO Miasto (Nazwa,Latitude,Longitude) VALUES('Warszawa',52.22972222,21.01166667)");
            Sql("INSERT INTO Miasto (Nazwa,Latitude,Longitude) VALUES('Wroc³aw',51.10000000,17.03305556)");
            Sql("INSERT INTO Miasto (Nazwa,Latitude,Longitude) VALUES('Zielona Góra',51.93527778,15.50638889)");
        }
        
        public override void Down()
        {
        }
    }
}
