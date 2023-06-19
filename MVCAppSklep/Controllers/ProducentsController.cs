using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCAppSklep.Data;
using MVCAppSklep.Models;

namespace MVCAppSklep.Controllers
{
    public class ProducentsController : Controller
    {
        private readonly SklepDbContext _context;

        public ProducentsController(SklepDbContext context)
        {
            _context = context;
        }

        // GET: Producents
        public async Task<IActionResult> Index()
        {
              return _context.Producenci != null ? 
                          View(await _context.Producenci.ToListAsync()) :
                          Problem("Entity set 'SklepDbContext.Producenci'  is null.");
        }

        // GET: Producents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Producenci == null)
            {
                return NotFound();
            }

            var producent = await _context.Producenci
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producent == null)
            {
                return NotFound();
            }

            return View(producent);
        }

        // GET: Producents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Producents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nazwa")] Producent producent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(producent);
        }

        // GET: Producents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Producenci == null)
            {
                return NotFound();
            }

            var producent = await _context.Producenci.FindAsync(id);
            if (producent == null)
            {
                return NotFound();
            }
            return View(producent);
        }

        // POST: Producents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nazwa")] Producent producent)
        {
            if (id != producent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProducentExists(producent.Id))
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
            return View(producent);
        }

        // GET: Producents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Producenci == null)
            {
                return NotFound();
            }

            var producent = await _context.Producenci
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producent == null)
            {
                return NotFound();
            }

            return View(producent);
        }

        // POST: Producents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Producenci == null)
            {
                return Problem("Entity set 'SklepDbContext.Producenci'  is null.");
            }
            var producent = await _context.Producenci.FindAsync(id);
            if (producent != null)
            {
                _context.Producenci.Remove(producent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProducentExists(int id)
        {
          return (_context.Producenci?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
