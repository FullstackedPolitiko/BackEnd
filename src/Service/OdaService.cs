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

             
            if (filter.SpecifikkeIds != null && filter.SpecifikkeIds.Any())
            {
                var idFiltre = filter.SpecifikkeIds.Select(id => $"Id eq {id}");
                

                var samletIdFilter = string.Join(" or ", idFiltre);
                

                query = query.Filter(samletIdFilter);
            }
            else 
            {
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

                Politikere = sag.SagAktør?
                    .Where(x => x.Aktør != null)
                    .Select(x => x.Aktør!.Navn)
                    .ToList() ?? new List<string>(),

                DokumentTitler = new List<string>() 
                
            }).ToList();

        return dtoListe;  
          }

public async Task<List<SagDTO>> GetSagerByPartyAndPeriode(string partyShortName, OdaPeriod period)
{
    var politikere = await GetPoliticalPartyMembers(partyShortName, period);
    
    if (politikere == null || !politikere.Any())
    {
        return new List<SagDTO>();
    }


    var sagTilPolitikerNavne = new Dictionary<int, List<string>>();

    foreach (var person in politikere)
    {
        try 
        {
            var links = await _client.For<SagAktør>("Sagaktør")
                .Filter(aktør => aktør.Aktørid == person.Id) 
                .FindEntriesAsync();

            foreach (var link in links)
            {
                if (!sagTilPolitikerNavne.ContainsKey(link.Sagid))
                {
                    sagTilPolitikerNavne[link.Sagid] = new List<string>();
                }
                
                if (!sagTilPolitikerNavne[link.Sagid].Contains(person.Navn))
                {
                    sagTilPolitikerNavne[link.Sagid].Add(person.Navn);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Kunne ikke hente sags-links for {person.Navn}: {ex.Message}");
        }
    }


    int pId = (int)period;
    var sagerFraPerioden = new List<Sag>();
    int skip = 0;
    int batchSize = 100;
    bool moreData = true;

    while (moreData)
    {
        try 
        {
            var batch = await _client.For<Sag>("Sag")
                .Filter(sag => sag.Periodeid == pId)
                .Skip(skip)
                .Top(batchSize)
                .FindEntriesAsync();

            var liste = batch.ToList();
            if (liste.Any())
            {
                sagerFraPerioden.AddRange(liste);
                skip += batchSize;
            }
            else
            {
                moreData = false;
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Fejl under hentning af sager (Skip: {skip}): {ex.Message}");
            Console.ResetColor();
            break;
        }
    }

    var partietsSagerIPerioden = sagerFraPerioden
        .Where(sag => sagTilPolitikerNavne.ContainsKey(sag.Id))
        .ToList();


    var dtoListe = partietsSagerIPerioden.Select(sag => new SagDTO
    {
        Sagsnummer = sag.Id,
        Overskrift = sag.Titel,
        KortResume = sag.Resume,
        Type = sag.Titelkort,
        SidstOpdateret = sag.Opdateringsdato,
        Politikere = sagTilPolitikerNavne[sag.Id],
        DokumentTitler = new List<string>()
    }).ToList();

    return dtoListe;
}
}