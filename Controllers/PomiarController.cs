using Microsoft.AspNet.Identity;
using ProjektTitsOI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.SqlServer;

namespace ProjektTitsOI.Controllers
{
    public enum OpisPogody
    {
        Bezchmurnie,
        CzescioweZachmurzenie,
        ZachmurzenieCalkowite,
    }
    public class PomiarController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        ApplicationDbContext _context;
        public PomiarController()
        {
            _context = new ApplicationDbContext();
        }
        private IEnumerable<string> GetOpisyPogody()
        {
            return new List<string>
            {
                "Bechmurnie",
                "Czesciowe Zachmurzenie",
                "Zachmurzenie calkowite"
            };
        }
        public PomiarController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        [Route("Pomiar/WszystkiePomiaryDlaGospodarstwa/{id}")]
        public ActionResult WszystkiePomiaryDlaGospodarstwa(int? id)
        {
            string iduser = User.Identity.GetUserId();
            var nazwa = (from g in _context.Godpodarstwo
                         where g.GospodarstwoId == id
                         select g.NazwaGospodarstwa);
            ViewBag.Message = String.Format("Pomiary dla gospodarstwa: {0}", nazwa.ToString());
            var query = (from p in _context.Pomiar
                         join g in _context.Godpodarstwo
                         on p.GospodarstwoId equals g.GospodarstwoId
                         where (p.GospodarstwoId == id)
                         select new
                         {
                             p.GospodarstwoId,
                             g.NazwaGospodarstwa,
                             p.Data,
                             p.PomiarId,
                             p.EnergiaWyprodukowana,
                             p.EnergiaZuzyta,
                             p.Dlugoscdnia,
                             p.Pogoda
                         });

            dynamic model = new ExpandoObject();
            List<ExpandoObject> joinData = new List<ExpandoObject>();
            foreach (var item in query)
            {
                IDictionary<string, object> itemExpando = new ExpandoObject();
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(item.GetType()))
                {
                    itemExpando.Add(property.Name, property.GetValue(item));
                }
                joinData.Add(itemExpando as ExpandoObject);
            }
            model = joinData;
            ViewBag.IdGospodarstwa = id;
            return View(model);
        }

        [HttpGet]
        [Route("Pomiar/DodajPomiarStrona/{id}")]
        public ActionResult DodajPomiarStrona(int? id)
        {
            List<string> OpisyPogody = new List<string>() { "Bezchmurnie", "Zachmurzenie Częściowe", "Zachmurzenie całkowite" };
            ViewBag.OpisyPogodyList = new SelectList(OpisyPogody);
            Pomiar pomiar = new Pomiar();
            pomiar.GospodarstwoId = (int)id;

            return View(pomiar);
        }
        [HttpPost]
        public ActionResult DodajPomiarStrona(Pomiar model)
        {
            var temp4 = model.Data;
            var minimalnadata = new DateTime(2000, 1, 1, 0, 0, 0);
            var maksymalnadata = new DateTime(2200, 1, 1, 0, 0, 0, 0);
            var porownaniedominimalnej = DateTime.Compare(temp4, minimalnadata);
            var porownaniedomaksymalnej = DateTime.Compare(temp4, maksymalnadata);
            if (porownaniedominimalnej <= 0 || porownaniedomaksymalnej >= 0)
            {
                return RedirectToAction(String.Format("DataPozaZakresem/{0}", model.GospodarstwoId));
            }

            bool CzyDataSiePowtarza = false;
            var indexpowtarzajacego = 0;
            var lista = _context.Pomiar.Where(x => x.GospodarstwoId == model.GospodarstwoId).Select(x => x);
            foreach (var item in lista)
            {
                if (item.Data == model.Data)
                {
                    CzyDataSiePowtarza = true;
                    indexpowtarzajacego = item.PomiarId;
                    break;
                }
            }

            if (CzyDataSiePowtarza == true)
            {
                return RedirectToAction("PowtarzajacaSieData", "Pomiar", new { id = indexpowtarzajacego });
            }
            else
            {
                var pomiar = new Pomiar { EnergiaWyprodukowana = model.EnergiaWyprodukowana, EnergiaZuzyta = model.EnergiaZuzyta, Pogoda = model.Pogoda, GospodarstwoId = model.GospodarstwoId, Data = model.Data };
                var temp = pomiar.Data.Day + "." + pomiar.Data.Month + "." + pomiar.Data.Year;
                string url = StworzUrl(temp, model.GospodarstwoId);
                var test = PobierzZApi(url, "day_length");
                double temp2 = double.Parse(test) / 3600;
                pomiar.Dlugoscdnia = temp2;
                _context.Pomiar.Add(pomiar);
                _context.SaveChanges();
                return RedirectToAction(String.Format("WszystkiePomiaryDlaGospodarstwa/{0}", pomiar.GospodarstwoId));

            }
        }

        [Route("Pomiar/EdytujPomiar/{id}")]
        public ActionResult EdytujPomiar(int? id)
        {

            List<string> OpisyPogody = new List<string>() { "Bezchmurnie", "Zachmurzenie Częściowe", "Zachmurzenie całkowite" };
            ViewBag.OpisyPogodyList = new SelectList(OpisyPogody);
            var pomiar = _context.Pomiar.Find(id);
            ViewBag.pomiaredytowany = pomiar;
            return View(pomiar);
        }

        [HttpPost]
        public ActionResult EdytujPomiar(Pomiar model)
        {
            var temp4 = model.Data;
            var minimalnadata = new DateTime(2000, 1, 1, 0, 0, 0);
            var maksymalnadata = new DateTime(2200, 1, 1, 0, 0, 0, 0);
            var porownaniedominimalnej = DateTime.Compare(temp4, minimalnadata);
            var porownaniedomaksymalnej = DateTime.Compare(temp4, maksymalnadata);
            if (porownaniedominimalnej <= 0 || porownaniedomaksymalnej >= 0)
            {
                return RedirectToAction(String.Format("DataPozaZakresemDlaEdycji/{0}", model.PomiarId));
            }

            if (ModelState.IsValid)
            {
                var pomiar = _context.Pomiar.Find(model.PomiarId);

                try
                {
                    if (ModelState.IsValid)
                    {

                        _context.Pomiar.Find(model.PomiarId).Data = model.Data;
                        _context.Pomiar.Find(model.PomiarId).EnergiaWyprodukowana = model.EnergiaWyprodukowana;
                        _context.Pomiar.Find(model.PomiarId).EnergiaZuzyta = model.EnergiaZuzyta;
                        _context.Pomiar.Find(model.PomiarId).Pogoda = model.Pogoda;
                        _context.SaveChanges();
                        return RedirectToAction(String.Format("WszystkiePomiaryDlaGospodarstwa/{0}", model.GospodarstwoId));
                    }
                }

                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }

            }
            return RedirectToAction("Index", "Home");

        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        private IEnumerable<SelectListItem> GetSelectListItems(IEnumerable<string> elements)
        {
            // Create an empty list to hold result of the operation
            var selectList = new List<SelectListItem>();

            // For each string in the 'elements' variable, create a new SelectListItem object
            // that has both its Value and Text properties set to a particular value.
            // This will result in MVC rendering each item as:
            //     <option value="State Name">State Name</option>
            foreach (var element in elements)
            {
                selectList.Add(new SelectListItem
                {
                    Value = element,
                    Text = element
                });
            }

            return selectList;
        }


        public ActionResult UsunPomiar(int? id, bool? saveChangesError = false)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            Pomiar pomiar = _context.Pomiar.Find(id);
            if (pomiar == null)
            {
                return HttpNotFound();
            }
            return View(pomiar);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UsunPomiar(Pomiar model)
        {

            try
            {
                Pomiar pomiar = _context.Pomiar.Find(model.PomiarId);
                _context.Pomiar.Remove(pomiar);
                _context.SaveChanges();
                return RedirectToAction(String.Format("WszystkiePomiaryDlaGospodarstwa/{0}", pomiar.GospodarstwoId));

            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("UsunPomiar", new { id = model.PomiarId, saveChangesError = true });
            }

        }
        public static string PobierzZApi(string url, string copobrac)
        {
            using (var w = new WebClient())
            {
                string napis = "";
                var json_data = string.Empty;
                try
                {
                    json_data = w.DownloadString(url);
                }
                catch (Exception) { }
                var jobject = (JObject)JsonConvert.DeserializeObject(json_data);
                napis = (string)(JValue)jobject["results"][copobrac];
                return napis;
            }
        }
        public string StworzUrl(string data, int id)
        {
            var napis = "https://api.sunrise-sunset.org/json?";
            var miastoid = _context.Godpodarstwo.Find(id).MiastoId;
            var latitude = _context.Miasta.Find(miastoid).Latitude.ToString();
            var longitude = _context.Miasta.Find(miastoid).Longitude.ToString();
            var lat = "lat=" + latitude;
            var lng = "lng=" + longitude;
            var date = "date=" + data;
            var formatka = "formatted=0";
            string url = napis + lat + "&" + lng + "&" + date + "&" + formatka;
            return url;
        }
        public ActionResult PowtarzajacaSieData(int? id)
        {
            Pomiar pomiar = _context.Pomiar.Find(id);
            return View(pomiar);
        }

        public ActionResult DataPozaZakresem(int? id)
        {

            ViewBag.GospodarstwoId = id;
            return View();
        }

        public ActionResult DataPozaZakresemDlaEdycji(int? id)
        {
            Pomiar pomiar = _context.Pomiar.Find(id);

            return View(pomiar);
        }
        JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
        [Route("Pomiar/WykresWszystkichPomiarow/{id}")]
        public ActionResult WykresWszystkichPomiarow(int? id)
        {

            var lista = _context.Pomiar.Where(x => x.GospodarstwoId == id).Select(x => x.EnergiaWyprodukowana).ToList();

            var lista2 = JsonConvert.SerializeObject(_context.Pomiar.Where(x => x.GospodarstwoId == id).Select(x => new { x.EnergiaWyprodukowana, V = x.Data }).ToList(), _jsonSetting);

            var temp = _context.Pomiar
                .Where(x => x.GospodarstwoId == id).OrderBy(x => x.Data).ToList()
                .Select(x => new { x.EnergiaWyprodukowana, V = x.Data.ToString("MM/dd/yyyy") });
            var temp2 = temp.ToList();
            if (temp2.Count == 0)
            {
                return RedirectToAction(String.Format("BrakPomiarowDlaWykresu/{0}", id), "Gospodarstwo");
            }
            ViewBag.DataPoints = JsonConvert.SerializeObject(temp, _jsonSetting);

            return View();

        }
        [Route("Pomiar/WykresWszystkichPomiarowEnergiaZuzyta/{id}")]

        public ActionResult WykresWszystkichPomiarowEnergiaZuzyta(int? id)
        {
            var temp = _context.Pomiar
                .Where(x => x.GospodarstwoId == id).OrderBy(x => x.Data).ToList()
                .Select(x => new { x.EnergiaZuzyta, V = x.Data.ToString("MM/dd/yyyy") });
            var temp2 = temp.ToList();
            if (temp2.Count == 0)
            {
                return RedirectToAction(String.Format("BrakPomiarowDlaWykresu/{0}", id), "Gospodarstwo");
            }
            ViewBag.DataPoints = JsonConvert.SerializeObject(temp, _jsonSetting);
            ViewBag.GospodarstwoId = id;
            return View();

        }
        [Route("Pomiar/WykresRocznyEnergiaWyprodukowanaZuzyta/{id}/{rok}")]

        public ActionResult WykresRocznyEnergiaWyprodukowanaZuzyta(int id, int rok)
        {

            var temp = _context.Pomiar
                .Where(x => x.GospodarstwoId == id && x.Data.Year == rok).OrderBy(x => x.Data).ToList()
                .Select(x => new { x.EnergiaZuzyta, x.EnergiaWyprodukowana, V = x.Data.ToString("MM/dd/yyyy") });
            var temp2 = temp.ToList();
            if (temp2.Count == 0)
            {
                return RedirectToAction(String.Format("BrakPomiarowDlaWykresu/{0}", id), "Gospodarstwo");
            }
            ViewBag.DataPoints = JsonConvert.SerializeObject(temp, _jsonSetting);
            ViewBag.GospodarstwoId = id;
            return View();
        }


    }
}
