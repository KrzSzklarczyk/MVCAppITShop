using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MVCAppSklep.Models;

public class Koszyk
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("ProduktId")]
    public int ProduktId { get; set; }
    [ValidateNever]
    public Produkt? Produkt { get; set; }

    [Display(Name = "Zamowiona Ilość")]
    public int ZamowionaIlosc { get; set; }

    [ForeignKey("AppUserID")]
    public string AppUserID { get; set; }
    [ValidateNever]
    public AppUser? AppUser { get; set; }

    [NotMapped]
    public double Price { get; set; }
}