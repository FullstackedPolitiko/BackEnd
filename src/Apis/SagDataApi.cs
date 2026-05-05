using ODA.model.oda;
using ODA.Service;
using src.model.Dto;
namespace src.Apis;

public static class SagDataApi
{
    public static IEndpointRouteBuilder MapSagDataApi(this IEndpointRouteBuilder app, bool isDevelopment)
    {
        var api = app.MapGroup("api/sager").WithTags("Sager");

        var getSagById = api.MapGet("/{id}", GetSagById)
            .WithName("GetSagById")
            .WithDescription("Hent en specifik politisk sag ud fra dens ID");
        
        var getFilteredSager = api.MapGet("/filter", GetFilteredSager)
            .WithName("GetFilteredSager")
            .WithDescription("Søg og filtrer i sager baseret på periode, søgeord mm.");
        
        var getSagerByParty = api.MapGet("/parti/{partyShortName}/{periode}",GetSagerByParty)
            .WithName("GetSagerByParty")
            .WithDescription("Hent alle unikke sager tilknyttet et bestemt parti i en given periode");
        
        var getAllSagerByPartyEver = api.MapGet("/parti/{partyShortName}/alle/{periode}", GetAllSagerByPartyEver)
            .WithName("GetAllSagerByPartyEver")
            .WithDescription("Hent alle sager på tværs af alle år, for politikere der er aktive i den valgte periode");

        if (!isDevelopment)
        {
            getSagById.RequireAuthorization();
            getFilteredSager.RequireAuthorization();
            getSagerByParty.RequireAuthorization();
            getAllSagerByPartyEver.RequireAuthorization();
        }

        return app;
    }

    public static async Task<IResult> GetSagById(int id, OdaService odaService)
    {
      var Sag = await odaService.GetPoliticalSag(id);
    
      if(Sag == null)
        {
            return Results.NotFound($"could not find a case with the ID {id}");
        }
        return Results.Ok(Sag);

    }
    public static async Task<List<SagDTO>> GetFilteredSager([AsParameters] SagFilterDTO filter, OdaService odaService)
    {
        return await odaService.GetFilteredSager(filter);
    }

    public static async Task<List<SagDTO>> GetSagerByParty(string partyShortName, OdaPeriod periode, OdaService odaService)
    {
        return await odaService.GetSagerByPartyAndPeriode(partyShortName, periode);
    }
    public static async Task<List<SagDTO>> GetAllSagerByPartyEver(
    string partyShortName, 
    OdaPeriod periode, 
    OdaService odaService)
{
    return await odaService.GetAllSagerByParty(partyShortName, periode);
}


}