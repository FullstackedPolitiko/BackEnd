using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;

namespace src.WebConfig
{
    public static class Cors
    {
        public static IServiceCollection SetupCors(this IServiceCollection app)
        {
            app.AddCors(options =>
            {
                options.AddPolicy("Allowed", policy =>
                {
                    policy.WithOrigins([
                        "https://www.politiskindsigt.dk",
                        "https://politiskindsigt.dk",
                    ])
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });

                 options.AddPolicy("Development", policy =>
                {
                    policy.WithOrigins([
                        "https://www.politiskindsigt.dk",
                        "https://politiskindsigt.dk",
                        "http://localhost:3000",
                        "http://localhost:5173"
                    ])
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            return app;
        }
    }
}