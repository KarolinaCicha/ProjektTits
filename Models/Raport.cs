using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjektTitsOI.Models
{
    public partial class DaneDoWykresu
    {
        public int x { get; set; }
        public Nullable<int> y { get; set; }
    }
    public class Raport
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        public Gospodarstwo Gospodarstwo { get; set; }
        public int GospodarstwoId { get; set; }
        public int LiczbaZarejestrowaychDni { get; set; }
        public int LiczbaDniPochmurnych { get; set; }
        public int LiczbaDniBezchmurnych { get; set; }
        public int LiczbaDniZCzesciowymZachmurzeniem { get; set; }
        public double SredniaWszystkichPomiarowEnergiaZuzyta { get; set; }
        public double SredniaWszystkichPomiarowEnergiaWyprodukowana{ get; set; }
        public double NajnizszyPomiarEnergiaWyprodukowana { get; set; }
        public double NajnizszyPomiarEnergiaZuzyta { get; set; }
        public double NajwyzszyPomiarEnergiaWyprodukowana { get; set; }
        public double NajwyzszyPomiarEnergiaZuzyta { get; set; }
        public DateTime DataNajnizszegoPomiaruEnergiaWyprodukowana { get; set; }
        public DateTime DataNajnizszegoPomiaruEnergiaZuzyta { get; set; }
        public DateTime DataNajwyzszegoPomiaruEnergiaWyprodukowana { get; set; }
        public DateTime DataNajwyzszegoPomiaruEnergiaZuzyta { get; set; }
        public int LiczbaDniZSaldemDodatnim { get; set; }
        public int LiczbaDniZSaldemUjemnym { get; set; }
        public int Rok { get; set; }
        public double CalaEnergiaWyprodukowana { get; set; }
        public double CalaEnergiaZuzyta { get; set; }
        public double CaleSaldo { get; set; }
        public double GodzinyDnia { get; set; }
        public double SrednieZuzycieNaGodzine { get; set; }
        public double SrednieProdukcjaNaGodzine { get; set; }

        public double SredniaEnergiaZuzytaNaOsobe { get; set; }
        public double SredniaEnergiaWyprodukowanaNaOsobe { get; set; }

     
        public double SredniaEnergiaWyprodukowanaNaPanele { get; set; }
        public double SredniaEnergiaWyprodukowanaNaPaneleNagodzine { get; set; }
        public Raport()
        {
            Raport raport;
        }
        public Raport(Gospodarstwo gospodarstwo)
        {
            this.Gospodarstwo = gospodarstwo;
            this.GospodarstwoId = gospodarstwo.GospodarstwoId;
            var ListaPomiarow = _context.Pomiar.Where(x => x.GospodarstwoId == gospodarstwo.GospodarstwoId);
            this.LiczbaZarejestrowaychDni = ListaPomiarow.Count();

            this.SredniaWszystkichPomiarowEnergiaZuzyta = ObliczSredniaEnergiZuzytej(ListaPomiarow);
            this.SredniaWszystkichPomiarowEnergiaWyprodukowana = ObliczSredniaEnergiWyprodukowanej(ListaPomiarow);

            this.NajnizszyPomiarEnergiaWyprodukowana = ListaPomiarow.Min(x => x.EnergiaWyprodukowana);
            var temp = ListaPomiarow.OrderBy(x => x.EnergiaWyprodukowana).First();
            this.DataNajnizszegoPomiaruEnergiaWyprodukowana= temp.Data;

            this.NajnizszyPomiarEnergiaZuzyta = ListaPomiarow.Min(x => x.EnergiaZuzyta);
            var temp2 = ListaPomiarow.OrderBy(x => x.EnergiaZuzyta).First();
            this.DataNajnizszegoPomiaruEnergiaZuzyta = temp2.Data;
            this.Rok = DateTime.Now.Year;
            this.NajwyzszyPomiarEnergiaWyprodukowana = ListaPomiarow.Max(x => x.EnergiaWyprodukowana);
            var temp3 = ListaPomiarow.OrderBy(x => x.EnergiaWyprodukowana).AsEnumerable().Last();
            this.DataNajwyzszegoPomiaruEnergiaWyprodukowana = temp3.Data;

            this.NajwyzszyPomiarEnergiaZuzyta = ListaPomiarow.Max(x => x.EnergiaZuzyta);
            var temp4 = ListaPomiarow.OrderBy(x => x.EnergiaZuzyta).AsEnumerable().Last();
            this.DataNajwyzszegoPomiaruEnergiaZuzyta = temp4.Data;

            this.LiczbaDniBezchmurnych = ListaPomiarow.Where(x => x.Pogoda == "Bezchmurnie").Count();
            this.LiczbaDniPochmurnych = ListaPomiarow.Where(x => x.Pogoda == "Zachmurzenie całkowite").Count();
            this.LiczbaDniZCzesciowymZachmurzeniem = ListaPomiarow.Where(x => x.Pogoda == "Zachmurzenie Częściowe").Count();

            this.LiczbaDniZSaldemUjemnym = ListaPomiarow.Select(x => x.EnergiaWyprodukowana - x.EnergiaZuzyta).Where(x => x < 0).Count();
            this.LiczbaDniZSaldemDodatnim = ListaPomiarow.Select(x => x.EnergiaWyprodukowana - x.EnergiaZuzyta).Where(x => x > 0).Count();

            this.CalaEnergiaWyprodukowana = ListaPomiarow.Sum(x => x.EnergiaWyprodukowana);
            this.CalaEnergiaZuzyta = ListaPomiarow.Sum(x => x.EnergiaZuzyta);
            this.CaleSaldo = this.CalaEnergiaWyprodukowana - this.CalaEnergiaZuzyta;

            this.GodzinyDnia = ListaPomiarow.Sum(x => x.Dlugoscdnia);

            this.SrednieZuzycieNaGodzine = this.CalaEnergiaZuzyta / this.GodzinyDnia;
            this.SrednieProdukcjaNaGodzine = this.CalaEnergiaWyprodukowana / this.GodzinyDnia;
            this.SredniaEnergiaWyprodukowanaNaOsobe = this.CalaEnergiaWyprodukowana / gospodarstwo.LiczbaOsob;

            this.SredniaEnergiaZuzytaNaOsobe = this.CalaEnergiaZuzyta / gospodarstwo.LiczbaOsob;
            this.SredniaEnergiaWyprodukowanaNaPanele = this.CalaEnergiaWyprodukowana / gospodarstwo.LiczbaPaneli;
            this.SredniaEnergiaWyprodukowanaNaPaneleNagodzine = (this.CalaEnergiaWyprodukowana / this.GodzinyDnia) / gospodarstwo.LiczbaPaneli;

        }


        public double ObliczSredniaEnergiZuzytej(IQueryable<Pomiar> lista)
        {
            double srednia;
            srednia = lista.Average(x => x.EnergiaZuzyta);
            return srednia;
        }
        public double ObliczSredniaEnergiWyprodukowanej(IQueryable<Pomiar> lista)
        {
            double srednia;
            srednia = lista.Average(x => x.EnergiaWyprodukowana);
            return srednia;
        }

    }
}