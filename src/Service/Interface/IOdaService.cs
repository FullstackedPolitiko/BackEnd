using ODA.model.oda;
using src.model.Dto;

namespace ODA.Service.Interface;

public interface IOdaService
{
    public Task<List<Politician>> GetPoliticalPartyMembers(string partyShortName, OdaPeriod period);
    Task<Sag?> GetSagAsync(int sagid);
    Task<List<Sagstrin>> GetSagstrinForSagAsync(int sagid);
}