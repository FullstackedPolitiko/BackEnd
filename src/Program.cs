using ODA.Service;
using src.Apis;
using src.WebConfig;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<OdaService>();
builder.Services.SetupCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors("Development");
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseCors("Allowed");
}

app.MapPoliticianDataApi();
app.Run();