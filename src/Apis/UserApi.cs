using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Database;
using src.model.Entities;

namespace src.Apis
{
    public static class UserApi
    {
        public static IEndpointRouteBuilder MapUserApi(this IEndpointRouteBuilder app)
        {
            var api = app.MapGroup("api/users");

            api.MapGet("/{id}", GetUserById)
            .WithName("user fetch")
            .WithDescription("Gets a user")
            .WithTags("fetch");

            api.MapPost("/create", CreateUser)
            .WithName("user create")
            .WithDescription("Creates a user")
            .WithTags("create");

            return app;
        }

        public static async Task<Ok<User>> GetUserById(int id, ApplicationDbContext db)
        {
            User user = null;
            user = await db.Users.SingleAsync(b => b.Id == id);

            return TypedResults.Ok(user);
        }

        public static async Task<Created<User>> CreateUser(
            [FromBody] User user,
            ApplicationDbContext db)
        {
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/users/{user.Id}", user);
        }
    }
}