using Data.Interfaces;
using Data.Repository;
using Microsoft.Extensions.DependencyInjection;
using Repository;

namespace ANPRVisionAPI.Extensions
{
    public static class RepositoryCollectionExtensions
    {
        public static IServiceCollection AddApplicationRepositories(this IServiceCollection services)
        {
            services.AddScoped<IFormRepository, FormRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IModuleRepository, ModuleRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IRolRepository, RolRepository>();
            services.AddScoped<IRolUserRepository, RolUserRepository>();
            services.AddScoped<IFormRolPermissionRepository, FormRolPermissionRepository>();
            services.AddScoped<IFormModuleRepository, FormModuleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IloginRepository, UserRepository>();
            return services;
        }
    }
}