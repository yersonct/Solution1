using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ANPRVisionAPI.Extensions
{
    public static class CorsCollectionExtensions
    {
        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var origenesPermitidos = configuration.GetValue<string>("OrigenesPermitidos")?.Split(",");
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy => policy
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
            return services;
        }
    }
}