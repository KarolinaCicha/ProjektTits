using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using ProjektTitsOI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ProjektTitsOI.Controllers
{
    public class GospodarstwoController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        ApplicationDbContext _context;
        //public static int? NumerIdAktualnegoGospodarstwa=0;
        
        // GET: Gospodarstwo
        public GospodarstwoController()
        {
            _context = new ApplicationDbContext();
        }
        public GospodarstwoController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _WszystkieGospodarstwa()
        {
            
            string id = User.Identity.GetUserId();
            ViewBag.Message = "Gospodarstwa";
            

            var query = (from g in _context.Godpodarstwo
                             join m in _context.Miasta
                             on g.MiastoId equals m.MiastoId
                             where g.ApplicationUser.Id == id
                             select new{ 
                                 g.GospodarstwoId,
                                 g.NazwaGospodarstwa,
                                 g.LiczbaPaneli,
                                 g.LiczbaOsob,
                                 g.MiastoNazwa,
                                 m.Nazwa });

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
            
            return PartialView(model);
        }
       


        [Route("Gospodarstwo/StronaGospodarstwa/{id}")]
        public ActionResult StronaGospodarstwa(int? id)
        {
            var gospodarstwo = _context.Godpodarstwo.Find(id);
            return View(gospodarstwo);
        }


        [Route("Gospodarstwo/SzczegolyGospodarstwa/{id}")]
        public ActionResult SzczegolyGospodarstwa(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gospodarstwo gospodarstwo = _context.Godpodarstwo.Find(id);
            if (gospodarstwo == null)
            {
                return HttpNotFound();
            }
            return View(gospodarstwo);
            
        }


        public ActionResult DodajGospodarstwo()
        {
            List<string> NazwyMiast = _context.Miasta.Select(x=>x.Nazwa).ToList();
                
                //new List<string>() 
             //{ "Bezchmurnie", "Zachmurzenie Częściowe", "Zachmurzenie całkowite" };
            ViewBag.NazwyMiast = new SelectList(NazwyMiast);
            Gospodarstwo gospodarstwo = new Gospodarstwo();
            
            return View(gospodarstwo);
        }


        [HttpPost]
      
        public ActionResult DodajGospodarstwo(Gospodarstwo model)
        {

            var gospodarstwo = new Gospodarstwo { NazwaGospodarstwa = model.NazwaGospodarstwa, LiczbaOsob=model.LiczbaOsob,LiczbaPaneli=model.LiczbaPaneli,MiastoNazwa=model.MiastoNazwa, UserId=model.UserId,ApplicationUser=model.ApplicationUser};
            var temp = _context.Miasta.Where(x => x.Nazwa == model.MiastoNazwa).Select(x=>x.MiastoId).First();
            gospodarstwo.MiastoId =temp;
            var temp2 = User.Identity.GetUserId();
            var applicationuser = _context.Users.Where(x => x.Id == temp2).First();
            gospodarstwo.ApplicationUser = applicationuser;
           
            try
            {

                
                
                _context.Godpodarstwo.Add(gospodarstwo);
                _context.SaveChanges();
               
                return RedirectToAction("Index", "Home");
             
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            


            return RedirectToAction("DodajPomiarStrona"); ;

        }
        public ActionResult UsunGospodarstwo(int? id, bool? saveChangesError = false)
        {
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
           Gospodarstwo gospodarstwo = _context.Godpodarstwo.Find(id);
            if (gospodarstwo == null)
            {
                return HttpNotFound();
            }
            return View(gospodarstwo);
        }
        [HttpPost]
       
        public ActionResult UsunGospodarstwo(Gospodarstwo model)
        {
            
            try
            {
               Gospodarstwo gospodarstwo = _context.Godpodarstwo.Find(model.GospodarstwoId);
                _context.Godpodarstwo.Remove(gospodarstwo);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");

            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("UsunGospodarstwo", new { id = model.GospodarstwoId, saveChangesError = true });
            }
           
        }

        public ActionResult EdytujGospodarstwo(int? id)
        {
            List<string> NazwyMiast = _context.Miasta.Select(x => x.Nazwa).ToList();
            ViewBag.NazwyMiast = new SelectList(NazwyMiast);
            Gospodarstwo gospodarstwo = _context.Godpodarstwo.Find(id);

            return View(gospodarstwo);
        }
        [HttpPost]
        public ActionResult EdytujGospodarstwo(Gospodarstwo model)
        {
            if (ModelState.IsValid)
            {
                var pomiar = _context.Pomiar.Find(model.GospodarstwoId);

                try
                { 
                    if (ModelState.IsValid)
                    {
                        _context.Godpodarstwo.Find(model.GospodarstwoId).NazwaGospodarstwa= model.NazwaGospodarstwa;
                        _context.Godpodarstwo.Find(model.GospodarstwoId).LiczbaPaneli= model.LiczbaPaneli;
                        _context.Godpodarstwo.Find(model.GospodarstwoId).LiczbaOsob = model.LiczbaOsob;
                        _context.Godpodarstwo.Find(model.GospodarstwoId).MiastoNazwa= model.MiastoNazwa;

                        _context.SaveChanges();
                     
                        return RedirectToAction("Index","Home");
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

        public ActionResult StronaZRaportem(int? id)
        {
            
            Gospodarstwo gospodarstwo = _context.Godpodarstwo.Find(id);
             List<Pomiar> lista = _context.Pomiar.Where(x => x.GospodarstwoId == id).ToList();
            if (lista.Count==0)
            {
                return RedirectToAction(String.Format("BrakPomiarow/{0}", id), "Gospodarstwo");


            }
            else
            { Raport raport = new Raport(gospodarstwo);
                return View(raport);
            }
            
           
        }
        public ActionResult BrakPomiarow(int? id)
        {
            Gospodarstwo gospodarstwo = _context.Godpodarstwo.Find(id);
            
            return View(gospodarstwo);
        }
        public ActionResult BrakPomiarowDlaWykresu(int? id)
        {
            Gospodarstwo gospodarstwo = _context.Godpodarstwo.Find(id);

            return View(gospodarstwo);
        }
        [HttpGet]
        [Route("Gospodarstwo/Wykresy/{id}")]
        public ActionResult Wykresy(int? id)
        {
            Gospodarstwo gospodarstwo = _context.Godpodarstwo.Find(id);
            var lista = _context.Pomiar.Where(x => x.GospodarstwoId == id).Select(x => x.EnergiaWyprodukowana).ToList();
            if (lista.Count == 0)
            {
                return RedirectToAction(String.Format("BrakPomiarow/{0}", id), "Gospodarstwo");
            }
            Raport raport  = new Raport(gospodarstwo);

            return View(raport);
        }
        [HttpPost]
       
        public ActionResult Wykresy(Raport raport)
        {
            
            Raport raport2 = raport;
            string napis = String.Format("WykresRocznyEnergiaWyprodukowanaZuzyta/{0}/{1}", raport.GospodarstwoId, raport.Rok);
            return RedirectToAction(napis, "Pomiar");
        }
        JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
       
    }
}


