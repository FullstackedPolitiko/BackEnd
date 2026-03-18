namespace ODA.model.oda;

public class SagAktør
{
    public int Id { get; set; }
    public int Aktørid { get; set; }
    public DateTime Opdateringsdato { get; set; }
    public int Rolleid { get; set; }
    public int Sagid { get; set; }
    
    
    //Navigation
    public Aktør Aktør { get; set; }
    public Sag Sag { get; set; }
}