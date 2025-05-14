using Business.Interfaces;
using Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ANPRVisionAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IFormService, FormService>();
            services.AddScoped<IParkingService, ParkingService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IModuleService, ModuleService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IRolService, RolService>();
            services.AddScoped<IRolUserService, RolUserService>();
            services.AddScoped<IRatesService, RatesService>();
            services.AddScoped<ITypeRatesService, TypeRatesService>();
            services.AddScoped<ICamaraService, CamaraService>();
            services.AddScoped<IMembershipsService, MembershipsService>();
            services.AddScoped<IFormRolPermissionService, FormRolPermissionService>();
            services.AddScoped<IFormModuleService, FormModuleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IBlackListService, BlackListService>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IRegisteredVehicleService, RegisteredVehicleService>();
            services.AddScoped<IMembershipsVehicleService, MembershipsVehicleService>();
            services.AddScoped<ITypeVehicleService, TypeVehicleService>();
            services.AddScoped<IVehicleHistoryService, VehicleHistoryService>();
            services.AddScoped<IVehicleHistoryParkingRatesService, VehicleHistoryParkingRatesService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IAuthService, AuthService>();
            return services;
        }
    }
}