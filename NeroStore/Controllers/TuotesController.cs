using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NeroStore.Models;

namespace NeroStore.Controllers
{
    public class TuotesController : Controller
    {
        private readonly NeroStoreDBContext _context;

        public TuotesController(NeroStoreDBContext context)
        {
            _context = context;
        }

        // GET: Tuotes
        public async Task<IActionResult> Index()
        {
            NeroStore.Apumetodit am = new Apumetodit(_context);
            if(am.OnkoSessiossa(this.HttpContext.Session) == true)
            {
                return View(await _context.Tuotes.ToListAsync());
            }
            return RedirectToAction("Kirjautuminen", "Home");
        }

        // GET: Tuotes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            NeroStore.Apumetodit am = new Apumetodit(_context);
            if(am.OnkoSessiossa(this.HttpContext.Session) == true)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var tuote = await _context.Tuotes
                    .FirstOrDefaultAsync(m => m.TuoteId == id);
                if (tuote == null)
                {
                    return NotFound();
                }
                ViewBag.Tuote = tuote;

                return View(tuote);
            }
            return RedirectToAction("Kirjautuminen", "Home");
        }

        // GET: Tuotes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tuotes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TuoteId,Nimi,Hinta,Lkm,Kuvaus,Tyyppi,Tuoteryhma")] Tuote tuote)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tuote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tuote);
        }

        // GET: Tuotes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tuote = await _context.Tuotes.FindAsync(id);
            if (tuote == null)
            {
                return NotFound();
            }
            return View(tuote);
        }

        // POST: Tuotes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TuoteId,Nimi,Hinta,Lkm,Kuvaus,Tyyppi,Tuoteryhma")] Tuote tuote)
        {
            if (id != tuote.TuoteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tuote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TuoteExists(tuote.TuoteId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tuote);
        }

        // GET: Tuotes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tuote = await _context.Tuotes
                .FirstOrDefaultAsync(m => m.TuoteId == id);
            if (tuote == null)
            {
                return NotFound();
            }

            return View(tuote);
        }

        // POST: Tuotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tuote = await _context.Tuotes.FindAsync(id);
            _context.Tuotes.Remove(tuote);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TuoteExists(int id)
        {
            return _context.Tuotes.Any(e => e.TuoteId == id);
        }
    }
}
