using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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

        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, NeroStoreDBContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            //var l = new Lasku(_configuration, _context);
            //l.LähetäLasku(new List<Tuote> { }, "foobar@hotmail.fi");

            return RedirectToAction("Etusivu");
        }

        public IActionResult Etusivu()
        {
            var a = new Apumetodit(_context);
            var katsotuimmatTuotteet = a.HaeKatsotuimmatTuotteet();
            ViewBag.KatsotuimmatTuotteet = katsotuimmatTuotteet;
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
        [Route("Home/Ostoskori/{tarkistettu}")]
        [Route("Home/Ostoskori")]

        public IActionResult Ostoskori(int? tarkistettu, string email = "", string checkboxPuuttuu = "", string emailPuuttuu = "",  string ostoskoriTyhjä = "")
        {
            Apumetodit db = new Apumetodit(_context);

            var minunOstoskori = db.HaeOstoskori(this.HttpContext.Session);
            ViewBag.KokonaisHinta = minunOstoskori.Select(a => a.Hinta).Sum();
            ViewBag.Lkm = minunOstoskori.Count();
            ViewBag.Email = email;
            ViewBag.CheckboxPuuttuu = checkboxPuuttuu;
            ViewBag.EmailPuuttuu = emailPuuttuu;
            ViewBag.OstoskoriTyhjä = ostoskoriTyhjä;
            ViewBag.Tarkistettu = tarkistettu;
            return View(minunOstoskori);
        }
        public IActionResult PoistaaKorista(int id)
        {
            Apumetodit db = new Apumetodit(_context);
            db.PoistaOstoskorista(this.HttpContext.Session, id);
            return RedirectToAction("Ostoskori", "Home");
        }
        [Route("Home/Ostoskori/{tarkistettu}")]
        [Route("Home/Ostoskori")]
        [HttpPost]
        public IActionResult Ostoskori(string email, string varmistus, string nappi)
        {
            Apumetodit am = new Apumetodit(_context);
            var sessio = this.HttpContext.Session;
            var ostoslista = am.HaeOstoskori(sessio);
            var varoitusteksti = "*Pakollinen kenttä";
            var varoitusOstoskoriTyhjä = "Ostoskorisi on tyhjä.";

            if (ostoslista.Count == 0)
            {
                return RedirectToAction("Ostoskori", new { Email = email, CheckboxPuuttuu = "", EmailPuuttuu = "",  OstoskoriTyhjä = varoitusOstoskoriTyhjä });
            }
            else if (varmistus != "Kyllä" && email == null)
            {
                ModelState.AddModelError("", "XXX");
                return RedirectToAction("Ostoskori", new { Email = email, CheckboxPuuttuu = varoitusteksti, EmailPuuttuu = varoitusteksti }) ;
            }
            else if (varmistus != "Kyllä") 
            {
                return RedirectToAction("Ostoskori", new { Email = email, CheckboxPuuttuu = varoitusteksti });
            }
            else if (email == null)
            {
                return RedirectToAction("Ostoskori", new { Email = email, CheckboxPuuttuu ="", EmailPuuttuu = varoitusteksti });
            }
            else
            {
                var kokonaissumma = ostoslista.Select(t => t.Hinta).Sum();
                Lasku lasku = new(_configuration, _context);
                lasku.LähetäLasku(ostoslista, email);

                am.LisääTilaus(email, kokonaissumma);
                foreach (var tuote in ostoslista)
                {
                    if (am.MuutaTuotteenSaldoa(tuote.TuoteId, -1))
                    {
                        am.LisaaTilausrivi(1, am.HaeViimeisimmänTilauksenId(), tuote.TuoteId);
                    }
                }
                return RedirectToAction("Kiitos");
            }
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

            if (kirjautuja != null)
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
        [Route("Home/Tietoja/{id}")]
       
        public IActionResult Tietoja(int id, string ostos = "")
        {
            var a = new Apumetodit(_context);
            a.LisääNäyttökerta(id);

            var tuote = _context.Tuotes.Where(t => t.TuoteId == id).FirstOrDefault();
            //ViewBag.Tuote = tuote;
            if(ostos == "ok")
            {
                ViewBag.Viesti = "Tuote lisätty ostoskoriin";
                return View(tuote);
            }
            return View(tuote);

        }

        public IActionResult Tuotteet(string kategoria = "")
        {
            List<Tuote> tuotteet;
            if (kategoria == "")
            {
                ViewBag.Otsikko = "Kaikki tuotteet";
                tuotteet = _context.Tuotes.Select(t => t).ToList();
            }
            else
            {
                ViewBag.Otsikko = "Kategorian " + kategoria + " tuotteet";
                ViewBag.Kategoria = kategoria;
                tuotteet = _context.Tuotes.Where(t => t.Tuoteryhma == kategoria).Select(t => t).ToList();
            }
            return View(tuotteet);
        }


        [Route("Home/LisääKoriin/{id}")]
        [Route("Home/LisääKoriin/{id}/{kategoria}")]
        public IActionResult LisääKoriin(int id, string kategoria = "")
        {
            Apumetodit am = new Apumetodit(_context);
            am.LisääOstoskoriin(this.HttpContext.Session, id);
            if (kategoria == "")
            {
                return RedirectToAction("Tuotteet", "Home");
            }
            else
            {
                return RedirectToAction("Tuotteet", "Home", new { kategoria = kategoria });
            }
        }
        public IActionResult LisääKoriinTietoja(int id)
        {
            Apumetodit am = new Apumetodit(_context);
            am.LisääOstoskoriin(this.HttpContext.Session, id);
            return RedirectToAction("Tietoja", "Home", new { id = id, ostos = "ok"});
        }

        public IActionResult Heikki()
        {
            return View();
        }

    }
}
