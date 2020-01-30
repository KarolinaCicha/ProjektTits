using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace ProjektTitsOI.Models
{
    public class Gospodarstwo
    {
        public int GospodarstwoId { get; set; }
        public string NazwaGospodarstwa { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Wpisz liczbę dodatnią")]
        public int LiczbaOsob { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Wpisz liczbę dodatnią")]
        public int LiczbaPaneli { get; set; }

        public int MiastoId { get; set; }
        public string MiastoNazwa { get; set; }
        public virtual ICollection<Pomiar> pomiary { get; set; }
        //[ForeignKey("dbo.AspNetUsers")]
        //  public virtual ApplicationUser ApplicationUser { get; set; }

        //public string UserId { get; set; }
        //[ForeignKey("UserId")]
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}