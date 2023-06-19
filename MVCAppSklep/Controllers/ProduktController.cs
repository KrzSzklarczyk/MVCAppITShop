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
    public class ProduktController : Controller
    {
        private readonly SklepDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public ProduktController(SklepDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Produkt
        public async Task<IActionResult> Index()
        {
            var sklepDbContext = _context.Produkty.Include(p => p.Kategoria).Include(p => p.Producer);
            return View(await sklepDbContext.ToListAsync());
        }

        // GET: Produkt/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Produkty == null)
            {
                return NotFound();
            }

            var produkt = await _context.Produkty
                .Include(p => p.Kategoria)
                .Include(p => p.Producer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produkt == null)
            {
                return NotFound();
            }

            return View(produkt);
        }

        // GET: Produkt/Create
        public IActionResult Create()
        {
            ViewData["KategoriaId"] = new SelectList(_context.Kategorie, "Id", "Nazwa");
            ViewData["ProducerId"] = new SelectList(_context.Producenci, "Id", "Nazwa");
            return View();
        }

        // POST: Produkt/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nazwa,Cena,Ilosc,ProducerId,KategoriaId,CoverImage")] Produkt produkt)
        {
            if (ModelState.IsValid)
            {
                if (produkt.CoverImage != null)
                {
                    produkt.CoverImageUrl = AddImage("produkty/okladki/", produkt.CoverImage);
                }
                _context.Add(produkt);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KategoriaId"] = new SelectList(_context.Kategorie, "Id", "Nazwa", produkt.KategoriaId);
            ViewData["ProducerId"] = new SelectList(_context.Producenci, "Id", "Nazwa", produkt.ProducerId);
            return View(produkt);
        }
        
        private string AddImage(string folderPath, IFormFile image)
        {
            folderPath += Guid.NewGuid().ToString() + image.FileName;
            string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);
            using (FileStream imageToCopy = new(imagePath, FileMode.Create))
            {
                image.CopyTo(imageToCopy);
            }
            return "/" + folderPath;
        }

        // GET: Produkt/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Produkty == null)
            {
                return NotFound();
            }

            var produkt = await _context.Produkty.FindAsync(id);
            if (produkt == null)
            {
                return NotFound();
            }
            ViewData["KategoriaId"] = new SelectList(_context.Kategorie, "Id", "Nazwa", produkt.KategoriaId);
            ViewData["ProducerId"] = new SelectList(_context.Producenci, "Id", "Nazwa", produkt.ProducerId);
            return View(produkt);
        }

        // POST: Produkt/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nazwa,Cena,Ilosc,ProducerId,KategoriaId,CoverImage")] Produkt produkt)
        {
            if (id != produkt.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(produkt.CoverImage == null)
                    {
                        produkt.CoverImageUrl = _context.Produkty.AsNoTracking().Where(x=>x.Id == id).Select(x=>x.CoverImageUrl).FirstOrDefault();
                    }
                    else
                    {
                        var oldProductCover = _context.Produkty.AsNoTracking().Where(x => x.Id == id).Select(x => x.CoverImageUrl).First();
                        if (System.IO.File.Exists(_webHostEnvironment.WebRootPath + oldProductCover))
                        {
                            System.IO.File.Delete(_webHostEnvironment.WebRootPath + oldProductCover);
                        }
                        produkt.CoverImageUrl = AddImage("produkty/okladki/", produkt.CoverImage);
                    }
                    _context.Update(produkt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProduktExists(produkt.Id))
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
            ViewData["KategoriaId"] = new SelectList(_context.Kategorie, "Id", "Nazwa", produkt.KategoriaId);
            ViewData["ProducerId"] = new SelectList(_context.Producenci, "Id", "Nazwa", produkt.ProducerId);
            return View(produkt);
        }

        // GET: Produkt/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Produkty == null)
            {
                return NotFound();
            }

            var produkt = await _context.Produkty
                .Include(p => p.Kategoria)
                .Include(p => p.Producer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produkt == null)
            {
                return NotFound();
            }

            return View(produkt);
        }

        // POST: Produkt/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Produkty == null)
            {
                return Problem("Entity set 'SklepDbContext.Produkty'  is null.");
            }
            var produkt = await _context.Produkty.FindAsync(id);
            if (produkt != null)
            {
                if (System.IO.File.Exists(_webHostEnvironment.WebRootPath + produkt.CoverImageUrl))
                {
                    System.IO.File.Delete(_webHostEnvironment.WebRootPath + produkt.CoverImageUrl);
                }
                _context.Produkty.Remove(produkt);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProduktExists(int id)
        {
          return (_context.Produkty?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
