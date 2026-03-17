using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.API.Configurations
{
    public static class CorsConfiguration
    {
        public static IServiceCollection AddCorsConfiguration(
        this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular", policy =>
                {
                    policy.WithOrigins(
                        "http://localhost:4200",  // Angular dev server
                        "http://localhost:80",     // Production
                        "https://yourdomain.com"   // Your domain
                    )
                    .AllowAnyMethod()              // GET, POST, PUT, DELETE
                    .AllowAnyHeader()              // Any headers
                    .AllowCredentials();           // Allow cookies/auth
                });
            });

            return services;
        }
    }
}
