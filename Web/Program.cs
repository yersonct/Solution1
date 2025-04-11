using Business;
using Data;
using Entity.Context;
using Entity.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Data;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // 🔹 Registrar el DbContext con PostgreSQL (esto maneja la conexión correctamente)
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        // 🔹 Registrar servicios de datos y negocios
        builder.Services.AddScoped<FormData>();
        builder.Services.AddScoped<FormBusiness>();

        builder.Services.AddScoped<ParkingData>();
        builder.Services.AddScoped<ParkingBusiness>();

        builder.Services.AddScoped<PersonData>();
        builder.Services.AddScoped<PersonBusiness>();

        builder.Services.AddScoped<ModuleData>();
        builder.Services.AddScoped<ModuleBusiness>();

        builder.Services.AddScoped<PermissionData>();
        builder.Services.AddScoped<PermissionBusiness>();
       
        builder.Services.AddScoped<RolData>();
        builder.Services.AddScoped<RolBusiness>();

        builder.Services.AddScoped<RolUserData>();
        builder.Services.AddScoped<RolUserBusiness>();

        builder.Services.AddScoped<RatesData>();
        builder.Services.AddScoped<RatesBusiness>(); 
        //builder.Services.AddScoped<RatesDTO>();

        builder.Services.AddScoped<TypeRatesData>();
        builder.Services.AddScoped<TypeRatesBusiness>();  

        builder.Services.AddScoped<CamaraData>();
        builder.Services.AddScoped<CamaraBusiness>();

        builder.Services.AddScoped<MembershipsData>();
        builder.Services.AddScoped<MembershipsBusiness>();

        builder.Services.AddScoped<FormRolPermissionBusiness>();
        builder.Services.AddScoped<FormRolPermissionData>();

        builder.Services.AddScoped<FormModuleData>();
        builder.Services.AddScoped<FormModuleBusiness>();

        builder.Services.AddScoped<UserData>();
        builder.Services.AddScoped<UserBusiness>();
        builder.Services.AddScoped<ClientData>();
        builder.Services.AddScoped<ClientBusiness>();

        builder.Services.AddScoped<BlackListData>();
        builder.Services.AddScoped<BlackListBusiness>();

        builder.Services.AddScoped<VehicleData>();
        builder.Services.AddScoped<VehicleBusiness>();

        builder.Services.AddScoped<RegisteredVehicleData>();
        builder.Services.AddScoped<RegisteredVehicleBusiness>();

        builder.Services.AddScoped<MembershipsVehicleData>();
        builder.Services.AddScoped<MembershipsVehicleBusiness>();

        builder.Services.AddScoped<TypeVehicleData>();
        builder.Services.AddScoped<TypeVehicleBusiness>();
        builder.Services.AddScoped<VehicleHistoryData>();
        builder.Services.AddScoped<VehicleHistoryBusiness>();

        builder.Services.AddScoped<VehicleHistoryParkingRatesData>();
        builder.Services.AddScoped<VehicleHistoryParkingRatesBusiness>();

        builder.Services.AddScoped<InvoiceData>();
        builder.Services.AddScoped<InvoiceBusiness>();
        // 🔹 Agregar servicios de controladores y documentación Swagger
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // 🔹 Configurar CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        });

        // 🔹 Configurar logging
        builder.Logging.AddConsole();

        // 🔹 Construir la aplicación
        var app = builder.Build();

        // 🔹 Aplicar middlewares
        app.UseCors("AllowAll");

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        // 🔹 Ejecutar la aplicación
        app.Run();
    }
}
    