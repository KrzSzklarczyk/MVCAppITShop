using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVCAppSklep.Models;

namespace MVCAppSklep.Data;

public class SklepDbContext : IdentityDbContext
{
    public SklepDbContext(DbContextOptions<SklepDbContext> options) : base(options)
    { }
    
    //tabele w bazie danych
    public DbSet<AppUser> Uzytkownicy { get; set; }
    public DbSet<Kategoria> Kategorie { get; set; }
    public DbSet<Koszyk> Koszyki { get; set; }
    public DbSet<Producent> Producenci { get; set; }
    public DbSet<Produkt> Produkty { get; set; }
    public DbSet<SzczegolyZamowienia> SzczegolyZamowien { get; set; }
    public DbSet<Zamowienie> Zamowienia { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}