using ODA.Service;
using src.Apis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<OdaService>();


var app = builder.Build();

app.MapPoliticianDataApi();

app.Run();