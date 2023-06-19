using System.ComponentModel.DataAnnotations;

namespace MVCAppSklep.Models;

public class Producent
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Nazwa { get; set; }
}