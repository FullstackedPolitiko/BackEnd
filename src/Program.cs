using Microsoft.EntityFrameworkCore;
using ODA.Service;
using src.Apis;
using src.Database;
using src.WebConfig;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
string ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddScoped<OdaService>();
builder.Services.SetupCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.SetupAuthentication();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(ConnectionString);
});

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
app.UseAuthentication(); 
app.UseAuthorization();  
app.MapUserApi();
app.MapPoliticianDataApi();

app.Run();