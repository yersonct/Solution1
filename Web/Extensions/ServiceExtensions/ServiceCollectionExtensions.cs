// Web/Extensions/ServiceExtensions/ServiceCollectionExtensions.cs

using Microsoft.Extensions.DependencyInjection;
using AutoMapper; // Necesario para AutoMapper
using Business.MapperProfiles; // Necesario para tu MappingProfile
using Business.Interfaces; // Tus interfaces de servicios
using Business.Services;   // Tus implementaciones de servicios

namespace ANPRVisionAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Configuración de AutoMapper
            // Registra todos los perfiles de AutoMapper que se encuentren en el ensamblado
            // donde está MappingProfile.
            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            // Registro de tus servicios de negocio (lo que ya tenías)
            services.AddScoped<IFormService, FormService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IModuleService, ModuleService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IRolService, RolService>();
            services.AddScoped<IRolUserService, RolUserService>();
            services.AddScoped<IFormRolPermissionService, FormRolPermissionService>();
            services.AddScoped<IFormModuleService, FormModuleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            
            return services;
        }
    }
}