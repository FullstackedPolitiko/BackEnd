namespace src.model.Dto;

public class SagDTO
{
    public int Sagsnummer { get; set; }
    public string Overskrift { get; set; } = string.Empty;
    public string? KortResume { get; set; }
    public string Type { get; set; } = string.Empty;
    public DateTime SidstOpdateret { get; set; }

    public List<string> Politikere { get; set; } = new();
    public List<string> DokumentTitler { get; set; } = new();
}