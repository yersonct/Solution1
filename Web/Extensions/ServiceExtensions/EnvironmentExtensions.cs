using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Business.Interfaces;

namespace ANPRVisionAPI.Extensions
{
    public static class EnvironmentExtensions
    {
        public static IApplicationBuilder ConfigureDevelopmentEnvironment(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var authService = services.GetRequiredService<IAuthService>();
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        Task.Run(async () => await authService.MigratePasswordsAsync()).Wait(); // Ejecutar la migración de forma asíncrona y esperar
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "Ocurrió un error durante la migración de contraseñas al inicio en el entorno de desarrollo.");
                    }
                }

                app.UseSwagger();
                app.UseSwaggerUI();
            }

            return app;
        }
    }
}