using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace src.WebConfig
{
    public static class Authentification 
    {
        public static IServiceCollection SetupAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://accounts.google.com";
                    options.Audience = "456252307245-h0562qe8uqj8crla1565pd6fbsgl62jb.apps.googleusercontent.com"; //Skal ændres til google acc
                    
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "https://accounts.google.com",
                        ValidateAudience = true,
                        ValidAudience = "456252307245-h0562qe8uqj8crla1565pd6fbsgl62jb.apps.googleusercontent.com",
                        ValidateLifetime = true
                    };
                });

            services.AddAuthorization();
            
            return services;
        }
    }
}