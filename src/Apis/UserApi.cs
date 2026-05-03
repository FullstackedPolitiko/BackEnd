using System.Security.Claims;
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

            api.MapGet("/login", GetOrCreateUser)
            .WithName("login")
            .WithDescription("logs in a user and creates it if first time logged in")
            .WithTags("login")
            .RequireAuthorization();

            return app;
        }

        public static async Task<IResult> GetOrCreateUser(ClaimsPrincipal claims, ApplicationDbContext db)
        {   
            var googleId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                        ?? claims.FindFirst("sub")?.Value;
            var email = claims.FindFirst(ClaimTypes.Email)?.Value;
            var name = claims.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(googleId)) return TypedResults.Unauthorized();

            var user = await db.Users.FirstOrDefaultAsync(u => u.GoogleID == googleId);

            if (user == null)
            {
                user = new User 
                { 
                    Name = name, 
                    Email = email,
                    GoogleID = googleId,
                };
                
                db.Users.Add(user);
                await db.SaveChangesAsync();
            }

            return TypedResults.Ok(user);
        }
    }
}