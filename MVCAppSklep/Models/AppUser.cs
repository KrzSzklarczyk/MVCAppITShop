using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MVCAppSklep.Models;

public class AppUser : IdentityUser
{
    [Required]
    public string Imie { get; set; }
    [Required]
    public string Nazwisko { get; set; }
    [Required]
    public string Miasto { get; set; }
    [Required]
    public string Adres { get; set; }
    [Required]
    [Display(Name = "Kod pocztowy")]
    public string KodPocztowy { get; set; }
    [Required]
    public string Kraj { get; set; }
}