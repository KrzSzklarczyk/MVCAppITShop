using System.Diagnostics;
using System.Numerics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCAppSklep.Data;
using MVCAppSklep.Models;

namespace MVCAppSklep.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly SklepDbContext _context;

    public HomeController(ILogger<HomeController> logger, SklepDbContext context)
    {
        _logger = logger;
        _context = context;
    }


    public IActionResult Index()
    {
        var produkty = _context.Produkty.Include(x => x.Producer)
            .Include(x => x.Kategoria).ToList();
        return View(produkty);
    }

    public IActionResult Details(int productId)
    {
        Koszyk koszyk = new Koszyk()
        {
            Produkt = _context.Produkty.Include(x => x.Kategoria).Include(x => x.Producer)
                .FirstOrDefault(x => x.Id == productId),
            ProduktId = productId,
            ZamowionaIlosc = 1
        };
        return View(koszyk);
    }
    
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public IActionResult Details(Koszyk koszyk)
    {
        var cl = (ClaimsIdentity)User.Identity;
        var claim = cl.FindFirst(ClaimTypes.NameIdentifier);
        koszyk.AppUserID = claim.Value;

        Koszyk cart = _context.Koszyki.FirstOrDefault(x => x.AppUserID == claim.Value && x.ProduktId == koszyk.ProduktId);
        if (cart == null)
        {
            _context.Koszyki.Add(koszyk);
        }
        else
        {
            cart.ZamowionaIlosc = koszyk?.ZamowionaIlosc + cart.ZamowionaIlosc ?? cart.ZamowionaIlosc;
            _context.Koszyki.Update(cart);
        }
        _context.SaveChanges();
        return RedirectToAction("Index", "Home");
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
}