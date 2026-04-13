using ODA.model.oda;
using ODA.Service;
using src.model.Dto;
namespace src.Apis
{
    public static class PoliticianDataApi
    {
        public static IEndpointRouteBuilder MapPoliticianDataApi(this IEndpointRouteBuilder app)
        {
            var api = app.MapGroup("api/PoliticianData");
            
            api.MapGet("/sager",GetSager)
            .WithName("Sager")
            .WithDescription("Get a paginated list of sager")
            .WithTags("Sager");

            return app;
        }

        public static async Task<List<Politician>> GetSager(
            string partyShortName, 
            OdaPeriod period,
            OdaService odaService)
        {
            return await odaService.GetPoliticalPartyMembers(partyShortName, period);
        }
    }
}