namespace ODA.model.oda;

public class Sag
{
  public string Afgørelse { get; set; }
  public DateTime? Afgørelsesdato { get; set; }
  public string Afgørelsesresultatkode { get; set; }
  public string Afstemningskonklusion { get; set; }
  public string Baggrundsmateriale { get; set; }
  public string Begrundelse { get; set; }
  public int? Deltundersagid { get; set; }
  public int? Fremsatundersagid { get; set; }
  public int Id { get; set; }
  public int Kategoriid { get; set; }
  public string Lovnummer { get; set; }
  public DateTime? Lovnummerdato { get; set; }
  public string Nummer { get; set; }
  public string Nummernumerisk { get; set; }
  public string Nummerpostfix { get; set; }
  public string Nummerprefix { get; set; }
  public string Offentlighedskode { get; set; }
  public DateTime Opdateringsdato { get; set; }
  public string Paragraf { get; set; }
  public int? Paragrafnummer { get; set; }
  public int Periodeid { get; set; }
  public string Resume { get; set; }
  public string Retsinformationsurl { get; set; }
  public DateTime? Rådsmødedato { get; set; }
  public bool Statsbudgetsag { get; set; }
  public int Statusid { get; set; }
  public string Titel { get; set; }
  public string Titelkort { get; set; }
  public int Typeid { get; set; }

  public List<Sagstrin>? Sagstrin { get; set; }

  public enum TypeId
  {
    UMF_DEL = 1,
    FORESPØRGSEL = 2,
    LOVFORESLAG = 3,
    ALM_DEL = 4,
    BESLUTNINGSFORSLAG = 5,
    RÅDSMØDE = 6,
    KOMMISIONSFORSLAG = 7,
    AKTSTYKKE = 8,
    FORSLAG_TIL_VEDTAGELSE = 9,
    PARAGRAF_20_SPØRGSMÅL = 10,
    REDEGØRELSE = 11,
    INDKALDELSE_AF_STEDFORTRÆDER = 12,
    STATSREVISORNE = 13
  }

  //Navigation
  public List<SagAktør> SagAktør { get; set; }
}
