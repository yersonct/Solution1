//using Microsoft.AspNetCore.Builder;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;

//namespace TuProyecto.Configuration // Ajusta el namespace a tu proyecto
//{
//    public static class CorsConfiguration
//    {
//        public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
//        {
//            services.AddCors(options =>
//            {
//                options.AddPolicy("AllowAngularApp",
//                    policy =>
//                    {
//                        var origenesPermitidos = configuration.GetSection("OrigenesPermitidos").Value;
//                        if (!string.IsNullOrEmpty(origenesPermitidos))
//                        {
//                            policy.WithOrigins(origenesPermitidos)
//                                   .AllowAnyMethod()
//                                   .AllowAnyHeader()
//                                   .AllowCredentials(); // Si necesitas manejar cookies o autenticación
//                        }
//                        else
//                        {
//                            // Configuración por defecto si no se especifica OrigenesPermitidos
//                            policy.AllowAnyOrigin()
//                                   .AllowAnyMethod()
//                                   .AllowAnyHeader();
//                        }
//                    });
//            });
//        }

//        public static void UseConfiguredCors(this IApplicationBuilder app)
//        {
//            app.UseCors("AllowAngularApp");
//        }
//    }
//}