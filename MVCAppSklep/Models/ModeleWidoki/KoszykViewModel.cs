namespace MVCAppSklep.Models.ModeleWidoki;

public class KoszykViewModel
{
    public IEnumerable<Koszyk> ListaKoszykow { get; set; }
    public Zamowienie Zamowienie { get; set; }
}