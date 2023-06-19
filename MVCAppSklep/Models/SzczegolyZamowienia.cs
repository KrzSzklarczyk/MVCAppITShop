using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MVCAppSklep.Models;

public class SzczegolyZamowienia
{
    [Key]
    public int Id { get; set; }
    public int Ilosc { get; set; }
    public double Cena { get; set; }
    
    [Display(Name = "Zamowienie")]
    public int ZamowienieId { get; set; }
    [ForeignKey("ZamowienieId")]
    [ValidateNever]
    public Zamowienie Zamowienie { get; set; }
    
    [Display(Name = "Produkt")]
    public int ProduktId { get; set; }
    [ForeignKey("ProduktId")]
    [ValidateNever]
    public Produkt Produkt { get; set; }
}