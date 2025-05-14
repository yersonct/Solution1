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
            services.AddScoped<IParkingRepository, ParkingRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IModuleRepository, ModuleRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IRolRepository, RolRepository>();
            services.AddScoped<IRolUserRepository, RolUserRepository>();
            services.AddScoped<IRatesRepository, RatesRepository>();
            services.AddScoped<ITypeRatesRepository, TypeRatesRepository>();
            services.AddScoped<ICamaraRepository, CamaraRepository>();
            services.AddScoped<IMembershipsRepository, MembershipsRepository>();
            services.AddScoped<IFormRolPermissionRepository, FormRolPermissionRepository>();
            services.AddScoped<IFormModuleRepository, FormModuleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IBlacklistRepository, BlacklistRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IRegisteredVehicleRepository, RegisteredVehicleRepository>();
            services.AddScoped<IMembershipsVehicleRepository, MembershipsVehicleRepository>();
            services.AddScoped<ITypeVehicleRepository, TypeVehicleRepository>();
            services.AddScoped<IVehicleHistoryRepository, VehicleHistoryRepository>();
            services.AddScoped<IVehicleHistoryParkingRatesRepository, VehicleHistoryParkingRatesRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IloginRepository, UserRepository>();
            return services;
        }
    }
}