using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MVCAppSklep.Models;

public class Zamowienie
{
    [Key]
    public int Id { get; set; }
    [Display(Name = "Cena Calkowita")]
    public double CenaCalkowita { get; set; }
    [Display(Name = "Data Zamowienia")]
    public DateTime DataZamowienia { get; set; } = DateTime.Now;
    [Display(Name = "Status Zamowienia")]
    public string StatusZamowienia { get; set; }
    
    [ForeignKey("AppUserID")]
    public string AppUserID { get; set; }
    [ValidateNever]
    public AppUser? AppUser { get; set; }
    
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