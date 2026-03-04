using ODA.model.oda;
using ODA.Service.Interface;
using Simple.OData.Client;

namespace ODA.Service;

public class OdaService : IOdaService
{
    private readonly ODataClient _client = new ODataClient("https://oda.ft.dk/api/");

    public async Task<List<Aktør>> GetPoliticalPartyMembers(string partyShortName, OdaPeriod period)
    {
        var partier = await _client.For<Aktør>()
            .Filter(x => x.Periodeid == (int) period)
            .Filter(x => x.Gruppenavnkort == partyShortName)
            .Expand(x => x.FraAktørAktør.Select(y => y.FraAktør))
            .FindEntriesAsync();

        return partier
            .SelectMany(p => p.FraAktørAktør ?? new List<AktørAktør>())
            .Select(rel => rel.FraAktør)
            .Where(person => person != null)
            .DistinctBy(person => person.Id)
            .OrderBy(person => person.Navn)
            .ToList();
    }
    
}