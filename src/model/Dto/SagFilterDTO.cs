using Microsoft.AspNetCore.Mvc; // VIGTIGT!

namespace src.model.Dto;




public class SagFilterDTO
{
    public int periodeId {get; set;}

    public string? Søgeord{get; set;}
    public int? TypeId { get; set;}
    [FromQuery]
    public int[]? SpecifikkeIds { get; set; }

}