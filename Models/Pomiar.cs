using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjektTitsOI.Models
{
    public class Pomiar
    {
        public int PomiarId { get; set; }
        [Required(ErrorMessage = "Nie ustalono daty")]
        // [Index(IsUnique = true)]
      
        [DataType(DataType.Date, ErrorMessage = "Wprowadź poprawną datę")]
       //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
      
        public DateTime Data { get; set; }
        [Required(ErrorMessage = "Wpisz wyprodukowaną energię")]
        [Range(0, int.MaxValue,ErrorMessage ="Wpisz liczbę dodatnią")]
        public int EnergiaWyprodukowana { get; set; }
        [Required(ErrorMessage = "Wpisz zużytą energię")]
        [Range(0, int.MaxValue, ErrorMessage = "Wpisz liczbę dodatnią")]
        public int EnergiaZuzyta { get; set; }
        public double Dlugoscdnia { get; set; }

        public string Pogoda { get; set; }
        //Foreign key for Gospodarstwo
        [ForeignKey("Gospodarstwo")]
        public int GospodarstwoId { get; set; }
        public Gospodarstwo Gospodarstwo { get; set; }
    }
}