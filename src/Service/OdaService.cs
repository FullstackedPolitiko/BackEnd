using ODA.model.oda;
using ODA.Service.Interface;
using Simple.OData.Client;
using src.model.Dto;

namespace ODA.Service;

public class OdaService : IOdaService
{
    private readonly ODataClient _client = new ODataClient("https://oda.ft.dk/api/");

    public async Task<List<Politician>> GetPoliticalPartyMembers(string partyShortName, OdaPeriod period)
    {
        var partier = await _client.For<Aktør>()
            .Filter(x => x.Periodeid == (int)period)
            .Filter(x => x.Gruppenavnkort == partyShortName)
            .Expand(x => x.FraAktørAktør.Select(y => y.FraAktør))
            .FindEntriesAsync();

        return partier
            .SelectMany(p => p.FraAktørAktør ?? new List<AktørAktør>())
            .Select(rel => rel.FraAktør)
            .Where(person => person != null)
            .DistinctBy(person => person.Id)
            .Select(person => new Politician
            {
                Id = person.Id,
                Fornavn = person.Fornavn,
                Efternavn = person.Efternavn,
                Navn = person.Navn,
                Gruppenavnkort = partyShortName,
            })
            .OrderBy(p => p.Navn)
            .ToList();
    }

    public async Task<Sag?> GetSagAsync(int sagid)
    {
        var result = await _client
            .For<Sag>()
            .Filter(x => x.Id == sagid)
            .FindEntriesAsync();

        return result.FirstOrDefault();
    }

    public async Task<List<Sagstrin>> GetSagstrinForSagAsync(int sagid)
    {
        var result = await _client
            .For<Sag>()
            .Key(sagid)
            .Expand(x => x.Sagstrin)
            .FindEntryAsync();

        return result?.Sagstrin?.ToList() ?? new List<Sagstrin>();
    }
    public async Task<List<Sag>> GetSagerForPartyAsync(string partyShortName, OdaPeriod period)
    {
        var result = await _client
            .For<Sag>()
            .Filter(x => x.Periodeid == (int)period)
            .Expand(x => x.SagAktør.Select(sa => sa.Aktør))
            .FindEntriesAsync();

        return result
            .Where(sag => sag.SagAktør != null &&
                          sag.SagAktør.Any(sa =>
                              sa.Aktør != null &&
                              sa.Aktør.Gruppenavnkort == partyShortName))
            .OrderBy(sag => sag.Titel)
            .ToList();
    }
        
    public async Task<SagDTO?> GetPoliticalSag(int id)
    {
        var sag = await _client
            .For<Sag>("Sag") 
            .Key(id)
            .Expand("Sagstrin", "Sagaktør/Aktør")
            .FindEntryAsync();

        if (sag == null) return null;

        var dto = new SagDTO
        {
            Sagsnummer = sag.Id,
            Overskrift = sag.Titel,
            KortResume = sag.Resume,
            Type = sag.Titelkort,
            SidstOpdateret = sag.Opdateringsdato,

            Politikere = sag.SagAktør?
            .Where(x => x.Aktør != null)
            .Select(x => x.Aktør!.Navn)
            .ToList() ?? new List<string>(),


        };
        
    return dto;
    }  

    public async Task<List<SagDTO>> GetFilteredSager(SagFilterDTO filter)
    {
        var query = _client.For<Sag>("Sag");

            // 1. Håndter specifikke ID'er 
            if (filter.SpecifikkeIds != null && filter.SpecifikkeIds.Any())
            {
                // Vi bygger en liste af strenge: ["Id eq 101", "Id eq 102"]
                var idFiltre = filter.SpecifikkeIds.Select(id => $"Id eq {id}");
                
                // Vi sætter dem sammen med " or ": "Id eq 101 or Id eq 102"
                var samletIdFilter = string.Join(" or ", idFiltre);
                
                // Vi sender tekststrengen til OData
                query = query.Filter(samletIdFilter);
            }
            else 
            {
                // Hvis ingen specifikke ID'er, så brug den normale periode-filtrering
                query = query.Filter(sag => sag.Periodeid == filter.periodeId);
            }



        if (!string.IsNullOrWhiteSpace(filter.Søgeord))
        {
            query = query.Filter(sag => sag.Titel.Contains(filter.Søgeord));
        }
        if (filter.TypeId.HasValue)
        {
            query = query.Filter(sag => sag.Typeid == filter.TypeId.Value);
        }
        var råSager = await query.Expand("Sagstrin", "Sagaktør/Aktør").FindEntriesAsync();
        var dtoListe = råSager.Select(sag => new SagDTO
            {
                Sagsnummer = sag.Id,
                Overskrift = sag.Titel,
                KortResume = sag.Resume,
                Type = sag.Titelkort,
                SidstOpdateret = sag.Opdateringsdato,

                // Vi plukker navnene ud af den komplekse struktur
                Politikere = sag.SagAktør?
                    .Where(x => x.Aktør != null)
                    .Select(x => x.Aktør!.Navn)
                    .ToList() ?? new List<string>(),

                // Vi lader dokumenter være tom, indtil du vil have dem med igen
                DokumentTitler = new List<string>() 
                
            }).ToList();

        return dtoListe;  
          }

    public async Task <List<Sag>> GetSaserByPartyAndPeriode(string partyShortName, OdaPeriod period)
    {
        // TRIN 1: Hent alle politikerne i partiet
        var politikere = await GetPoliticalPartyMembers(partyShortName, period);
        
        if (politikere == null || !politikere.Any())
        {
            Console.WriteLine("Fejl: Fandt ingen politikere for dette parti i denne periode.");
            return new List<Sag>();
        }

        Console.WriteLine($"Fandt {politikere.Count} politikere i {partyShortName}. Henter deres sager (dette kan tage et par sekunder)...");

        // TRIN 2: Opret en stor tom "kurv", vi kan lægge alle sagerne i
        var allePartietsSager = new List<Sag>();

        // TRIN 3: Spørg Folketinget om sager for HVER enkelt politiker
        foreach (var person in politikere)
        {
            var personensSager = await _client.For<Sag>("Sag")
                .Filter(sag => sag.Periodeid == (int)period && sag.SagAktør.Any(aktør => aktør.Aktørid == person.Id))
                .FindEntriesAsync();

            // Læg denne politikers sager ned i den store fælles kurv
            allePartietsSager.AddRange(personensSager);
        }

        
        var unikkeSager = allePartietsSager.DistinctBy(sag => sag.Id).ToList();

        return unikkeSager;
    }
}