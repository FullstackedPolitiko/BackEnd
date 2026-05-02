using ODA.model.oda;
using ODA.Service;
using src.model.Dto;
namespace src.Apis;


public static class SagDataApi
{
    public static IEndpointRouteBuilder MapSagDataApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/sager").WithTags("Sager");

        api.MapGet("/{id}", GetSagById)
            .WithName("GetSagById")
            .WithDescription("Hent en specifik politisk sag ud fra dens ID");
        
        api.MapGet("/filter", GetFilteredSager)
            .WithName("GetFilteredSager")
            .WithDescription("Søg og filtrer i sager baseret på periode, søgeord mm.");

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
}