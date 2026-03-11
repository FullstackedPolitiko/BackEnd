using Microsoft.EntityFrameworkCore;
using BackEnd.model;
namespace BackEnd.EndPoints;


public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        // 2. API Rute: Hent alle brugere
    app.MapGet("/users", async (AppDbContext db) =>
    {
        return await db.Users.ToListAsync();
    });

    // 3. API Rute: Opret en ny bruger
    app.MapPost("/users", async (User newUser, AppDbContext db) =>
    {
        db.Users.Add(newUser);
        await db.SaveChangesAsync();
        return Results.Created($"/users/{newUser.Id}", newUser);
    });
    }

}