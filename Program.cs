using Microsoft.EntityFrameworkCore;
using BackEnd.model;
using BackEnd.EndPoints;

var builder = WebApplication.CreateBuilder(args);

// 1. Forbind til PostgreSQL via Entity Framework
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();


app.MapUserEndpoints();

app.Run();