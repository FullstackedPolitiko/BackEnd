using ODA.model.oda;

namespace ODA.Service.Interface;

public interface IOdaService
{
    public Task<List<Aktør>> GetPoliticalPartyMembers(string partyShortName, OdaPeriod period);
}