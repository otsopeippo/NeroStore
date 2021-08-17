using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NeroStore.Models;
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
            var apustelijat = new Apumetodit(_context);
            //var sessio = HttpContext.Session;
            //apustelijat.LisääOstoskoriin(sessio, 1);
            //var testi = HttpContext.Session.GetString("foo");
            //var tuotteet = apustelijat.HaeTuotteet();

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
            return View();
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
        public IActionResult Kirjautuminen(string nimi, string salasana)
        {
            NeroStore.Apumetodit am = new Apumetodit(_context);
           
            if (am.KäyttäjäOnOlemassa(id) == true)
            {
                if (am.KäyttäjäOnAdmin(id) == true)
                {
                    return RedirectToAction("Index", "TuotesController");
                }

            }
            return View();
        }
    }
}
