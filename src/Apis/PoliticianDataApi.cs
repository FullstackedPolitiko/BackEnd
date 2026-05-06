using ODA.model.oda;
using ODA.Service;
using src.model.Dto;
namespace src.Apis
{
    public static class PoliticianDataApi
    {
        public static IEndpointRouteBuilder MapPoliticianDataApi(this IEndpointRouteBuilder app, bool isDevelopment)
        {
            var api = app.MapGroup("api/PoliticianData");
            
            var endpoint = api.MapGet("/politicians/{partyShortName}/{period}",GetPoliticians)
            .WithName("politicians")
            .WithDescription("Get a paginated list of politicians from a party")
            .WithTags("Politicians");

            if (!isDevelopment)
            {
                endpoint.RequireAuthorization();
            }

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

