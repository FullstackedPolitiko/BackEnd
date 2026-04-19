using ODA.Service;
using src.Apis;
using src.WebConfig;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<OdaService>();
builder.Services.SetupCors();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors("Development");
}
else
{
    app.UseCors("Allowed");
}

app.MapPoliticianDataApi();

app.Run();