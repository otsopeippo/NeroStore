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
            return View(db.HaeTuotteet());
        }

        public IActionResult Kiitos()
        {
            return View();
        }
    }
}
