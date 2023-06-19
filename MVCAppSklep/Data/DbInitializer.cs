using Microsoft.AspNetCore.Identity;
using MVCAppSklep.Models;

namespace MVCAppSklep.Data;

public class DbInitializer : IDbInitializer
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SklepDbContext _db;

    public DbInitializer(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SklepDbContext db) {
        _roleManager = roleManager;
        _userManager = userManager;
        _db = db;
    }
    public void Initialize()
    {
        if (!_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
        {
            _roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();

            _userManager.CreateAsync(new AppUser()
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                Imie = "Krzych",
                Nazwisko = "Szklarczyk",
                Miasto = "Trzebinia",
                Adres = "123",
                KodPocztowy = "123",
                Kraj = "Polska"
            }, "Admin123@").GetAwaiter().GetResult();
            var role = _roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();
            var user = _userManager.FindByEmailAsync("admin@admin.com").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(user, "Admin").GetAwaiter().GetResult();
        }
    }
}