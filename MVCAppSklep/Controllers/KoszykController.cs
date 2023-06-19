using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCAppSklep.Data;
using MVCAppSklep.Models;
using MVCAppSklep.Models.ModeleWidoki;

namespace MVCAppSklep.Controllers;
[Authorize]
public class KoszykController : Controller
{
    [BindProperty]
    public KoszykViewModel CartViewModel { get; set; }

    private readonly SklepDbContext _context;

    public KoszykController(SklepDbContext context)
    {
        _context = context;
    }
    
    
    // GET
    public IActionResult Index()
    {
        var cl = (ClaimsIdentity)User.Identity;
        var claim = cl.FindFirst(ClaimTypes.NameIdentifier);

        CartViewModel = new KoszykViewModel()
        {
            ListaKoszykow = _context.Koszyki.Where(x => x.AppUserID == claim.Value).Include(x => x.Produkt),
            Zamowienie = new()
        };

        foreach (var cart in CartViewModel.ListaKoszykow)
        {
            cart.Price = cart.Produkt.Cena * cart.ZamowionaIlosc;
            CartViewModel.Zamowienie.CenaCalkowita += cart.Price;
        }
        
        return View(CartViewModel);
    }

    public IActionResult Podsumowanie()
    {
        var cl = (ClaimsIdentity)User.Identity;
        var claim = cl.FindFirst(ClaimTypes.NameIdentifier);

        CartViewModel = new KoszykViewModel()
        {
            ListaKoszykow = _context.Koszyki.Where(x => x.AppUserID == claim.Value).Include(x => x.Produkt),
            Zamowienie = new()
        };
        
        CartViewModel.Zamowienie.AppUser = _context.Uzytkownicy.FirstOrDefault(x => x.Id == claim.Value);
        CartViewModel.Zamowienie.Imie = CartViewModel.Zamowienie.AppUser.Imie;
        CartViewModel.Zamowienie.Nazwisko = CartViewModel.Zamowienie.AppUser.Nazwisko;
        CartViewModel.Zamowienie.KodPocztowy = CartViewModel.Zamowienie.AppUser.KodPocztowy;
        CartViewModel.Zamowienie.Miasto = CartViewModel.Zamowienie.AppUser.Miasto;
        CartViewModel.Zamowienie.Kraj = CartViewModel.Zamowienie.AppUser.Kraj;
        CartViewModel.Zamowienie.Adres = CartViewModel.Zamowienie.AppUser.Adres;

        foreach (var cart in CartViewModel.ListaKoszykow)
        {
            cart.Price = cart.Produkt.Cena * cart.ZamowionaIlosc;
            CartViewModel.Zamowienie.CenaCalkowita += cart.Price;
        }
        return View(CartViewModel);
    }


    [HttpPost]
    [ActionName("Podsumowanie")]
    [ValidateAntiForgeryToken]
    public IActionResult PodsumowaniePOST()
    {
		var claimsIdentity = (ClaimsIdentity)User.Identity;
		var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
		AppUser appUser = _context.Uzytkownicy.FirstOrDefault(x => x.Id == userId);

        CartViewModel.ListaKoszykow = _context.Koszyki.Where(x => x.AppUserID == userId)
            .Include(x => x.Produkt).ToList();

        CartViewModel.Zamowienie.DataZamowienia = DateTime.Now;
        CartViewModel.Zamowienie.AppUserID = userId;

		foreach (var cart in CartViewModel.ListaKoszykow)
		{
			cart.Price = cart.Produkt.Cena * cart.ZamowionaIlosc;
			CartViewModel.Zamowienie.CenaCalkowita += cart.Price;
		}

        CartViewModel.Zamowienie.StatusZamowienia = "Wysyłanie";
        _context.Zamowienia.Add(CartViewModel.Zamowienie);
        _context.SaveChanges();

		foreach (var cart in CartViewModel.ListaKoszykow)
		{
			SzczegolyZamowienia orderDetail = new()
			{
				ZamowienieId = CartViewModel.Zamowienie.Id,
				ProduktId = cart.ProduktId,
				Ilosc = cart.ZamowionaIlosc,
				Cena = cart.Price
			};
			_context.SzczegolyZamowien.Add(orderDetail);
			_context.SaveChanges();
		}

        return RedirectToAction("Potwierdzenie", new { id = CartViewModel.Zamowienie.Id });
	}

    public IActionResult Potwierdzenie(int id)
    {
        Zamowienie zamowienie = _context.Zamowienia.Include(x => x.AppUser).FirstOrDefault(x => x.Id == id);

        List<Koszyk> koszyki = _context.Koszyki.Where(x => x.AppUserID == zamowienie.AppUserID).ToList();
        _context.Koszyki.RemoveRange(koszyki);
        _context.SaveChanges();

        return View(id);
    }

    public IActionResult Remove(int cartId)
    {
        var koszyk = _context.Koszyki.FirstOrDefault(x => x.Id == cartId);
        _context.Koszyki.Remove(koszyk);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
}