using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MVCAppSklep.Models;

public class Produkt
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Nazwa { get; set; }
    [Required]
    public double Cena { get; set; }
    [Required]
    public int Ilosc { get; set; }
    
    [Display(Name = "Producent")]
    public int ProducerId { get; set; }
    [ForeignKey("ProducerId")]
    [ValidateNever]
    public Producent? Producer { get; set; }
    
    [Display(Name = "KategoriaId")]
    public int KategoriaId { get; set; }
    [ForeignKey("KategoriaId")]
    [ValidateNever]
    public Kategoria? Kategoria { get; set; }
    
    [Display(Name = "Cover image")]
    [NotMapped]
    public IFormFile? CoverImage { get; set; }
    public string? CoverImageUrl { get; set; }
}