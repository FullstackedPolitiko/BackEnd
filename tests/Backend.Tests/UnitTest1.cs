using ODA.model.oda;
using ODA.Service; 

namespace Backend.Tests;

public class OdaServiceTests
{
    [Fact]
    public async Task GetPoliticalPartyMembers_ShouldReturnSortedListFromLiveApi()
    {
        var service = new OdaService(); 
        string party = "V"; 

        var result = await service.GetPoliticalPartyMembers(party, OdaPeriod.P2026);

        Assert.NotNull(result);
        if (result.Any())
        {
            Assert.True(string.Compare(result.First().Navn, result.Last().Navn) <= 0);
            
            var distinctCount = result.Select(p => p.Id).Distinct().Count();
            Assert.Equal(distinctCount, result.Count);
        }
    }
}