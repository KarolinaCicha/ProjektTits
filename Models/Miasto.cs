using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjektTitsOI.Models
{
    public class Miasto
    {
        public int MiastoId { get; set; }
        [Required(ErrorMessage = "Nie wpisano Nazwy ")]
        public string Nazwa { get; set; }
        [Required(ErrorMessage = "Nie wpisano Latitude")]
        public double Latitude { get; set; }
        [Required(ErrorMessage = "Nie wpisano Longtitude")]
        public double Longitude { get; set; }
    }
}