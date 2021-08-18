using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NeroStore.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace NeroStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly NeroStoreDBContext _context;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, NeroStoreDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            //var a = new Apumetodit(_context);
            //var sessio = HttpContext.Session;

            //a.LisääOstoskoriin(sessio, 2);
            //a.LisääOstoskoriin(sessio, 2);

            //var ostoskori = a.HaeOstoskori(sessio);
            //Console.WriteLine(ostoskori);

            return RedirectToAction("Etusivu");
        }

        public IActionResult Etusivu()
        {






            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Ostoskori()
        {
            Apumetodit db = new Apumetodit(_context);


            return View(db.HaeOstoskori(this.HttpContext.Session));
        }
        public IActionResult PoistaaKorista(int id)
        {
            Apumetodit db = new Apumetodit(_context);
            db.PoistaOstoskorista(this.HttpContext.Session, id);
            return RedirectToAction("Ostoskori", "Home");
        }

        public IActionResult Kiitos()
        {
            return View();
        }
        public IActionResult Kirjautuminen()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Kirjautuminen(string etunimi, string sukunimi, string salasana)
        {
            int id = 0;
            NeroStore.Apumetodit am = new Apumetodit(_context);

            var kirjautuja = _context.Kayttajas.Where(k => k.Etunimi == etunimi && k.Sukunimi == sukunimi).FirstOrDefault();

            if(kirjautuja != null)
            {

                if (kirjautuja.Salasana == salasana)
                {
                    id = kirjautuja.KayttajaId;
                    if (am.KäyttäjäOnOlemassa(id) == true)
                    {
                        if (am.KäyttäjäOnAdmin(id) == true)
                        {
                            am.LisääAdminSessioon(this.HttpContext.Session, id);
                            return RedirectToAction("Index", "Tuotes");
                        }
                    }
                }
            }

            return View();
        }
        public IActionResult Tietoja(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var tuote = _context.Tuotes.Where(t => t.TuoteId == id).FirstOrDefault();
                //ViewBag.Tuote = tuote;
                return View(tuote);
            }
        }
    }
}
