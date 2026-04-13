namespace ODA.model.oda;

public class Aktør
{
    public string Biografi { get; set; }
    public string Efternavn { get; set; }
    public string Fornavn { get; set; }
    public string Gruppenavnkort { get; set; }
    public int Id { get; set; }
    public string Navn { get; set; }
    public DateTime Opdateringsdato { get; set; }
    public int? Periodeid { get; set; }
    public DateTime? Slutdato { get; set; }
    public DateTime? Startdato { get; set; }
    public int Typeid { get; set; }
    public Aktørtype Aktørtype { get; set; }
    public List<SagAktør> SagAktør { get; set; }
    public List<Stemme> Stemme { get; set; }
    //Navigation
    public List<AktørAktør> TilAktørAktør { get; set; }
    public List<AktørAktør> FraAktørAktør { get; set; }
}
