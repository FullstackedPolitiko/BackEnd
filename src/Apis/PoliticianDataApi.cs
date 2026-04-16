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
            
            api.MapGet("/politicians/{partyShortName}/{period}",GetPoliticians)
            .WithName("politicians")
            .WithDescription("Get a paginated list of politicians from a party")
            .WithTags("Politicians");

            return app;
        }

        public static async Task<List<Politician>> GetPoliticians(
            string partyShortName, 
            OdaPeriod period,
            OdaService odaService)
        {
            return await odaService.GetPoliticalPartyMembers(partyShortName, period);
        }
    }
}