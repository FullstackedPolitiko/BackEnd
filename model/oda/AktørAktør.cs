namespace ODA.model.oda;

public class AktørAktør
{
    public int Fraaktørid { get; set; }
    public int Id { get; set; }
    public DateTime Opdateringsdato { get; set; }
    public int Rolleid { get; set; }
    public DateTime? Slutdato { get; set; }
    public DateTime? Startdato { get; set; }
    public int Tilaktørid { get; set; }
    public AktørAktørRolle AktørAktørRolle { get; set; }
    
    //Navigation
    public Aktør FraAktør { get; set; }
    public Aktør TilAktør { get; set; }
}
