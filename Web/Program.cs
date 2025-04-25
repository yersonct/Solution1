using Business;
using Business.Interfaces;
using Business.Services;
using Data;
using Data.Interfaces;
using Data.Repository;
using Entity.Context;
using Entity.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using Repository;
using System.Data;
using configuracion;
//using TuProyecto.Configuration; // Asegúrate de usar el namespace correcto


        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // 🔹 Registrar el DbContext con PostgreSQL
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        var origenesPermitidos = builder.Configuration.GetValue<string>("OrigenesPermitidos")!.Split(",");
        builder.Services.AddCors(opciones =>
        {
            opciones.AddDefaultPolicy(politica =>
            {
                politica.WithOrigins(origenesPermitidos).AllowAnyHeader().AllowAnyMethod();
            });
        });

        // 🔹 Registrar servicios de datos y negocios
        builder.Services.AddScoped<IFormService, FormService>();
        builder.Services.AddScoped<IFormRepository, FormRepository>();
        builder.Services.AddScoped<IParkingService, ParkingService>();
        builder.Services.AddScoped<IParkingRepository, ParkingRepository>();
        builder.Services.AddScoped<IPersonService, PersonService>();
        builder.Services.AddScoped<IPersonRepository, PersonRepository>();
        builder.Services.AddScoped<IModuleService, ModuleService>();
        builder.Services.AddScoped<IModuleRepository, ModuleRepository>();
        builder.Services.AddScoped<IPermissionService, PermissionService>();
        builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
        builder.Services.AddScoped<IRolService, RolService>();
        builder.Services.AddScoped<IRolRepository, RolRepository>();
        builder.Services.AddScoped<IRolUserService, RolUserService>();
        builder.Services.AddScoped<IRolUserRepository, RolUserRepository>();
        builder.Services.AddScoped<IRatesService, RatesService>();
        builder.Services.AddScoped<IRatesRepository, RatesRepository>();
        builder.Services.AddScoped<ITypeRatesService, TypeRatesService>();
        builder.Services.AddScoped<ITypeRatesRepository, TypeRatesRepository>();
        builder.Services.AddScoped<ICamaraService, CamaraService>();
        builder.Services.AddScoped<ICamaraRepository, CamaraRepository>();
        builder.Services.AddScoped<IMembershipsService, MembershipsService>();
        builder.Services.AddScoped<IMembershipsRepository, MembershipsRepository>();
        builder.Services.AddScoped<IFormRolPermissionService, FormRolPermissionService>();
        builder.Services.AddScoped<IFormRolPermissionRepository, FormRolPermissionRepository>();
        builder.Services.AddScoped<IFormModuleService, FormModuleService>();
        builder.Services.AddScoped<IFormModuleRepository, FormModuleRepository>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IClientService, ClientService>();
        builder.Services.AddScoped<IClientRepository, ClientRepository>();
        builder.Services.AddScoped<IBlackListService, BlackListService>();
        builder.Services.AddScoped<IBlacklistRepository, BlacklistRepository>();
        builder.Services.AddScoped<IVehicleService, VehicleService>();
        builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
        builder.Services.AddScoped<IRegisteredVehicleService, RegisteredVehicleService>();
        builder.Services.AddScoped<IRegisteredVehicleRepository, RegisteredVehicleRepository>();
        // 🔹 Registra el servicio MembershipsVehicleService y su repositorio
        builder.Services.AddScoped<MembershipsVehicleService, MembershipsVehicleService>(); // Corregido
        builder.Services.AddScoped<IMembershipsVehicleRepository, MembershipsVehicleRepository>();
        builder.Services.AddScoped<ITypeVehicleService, TypeVehicleService>();
        builder.Services.AddScoped<ITypeVehicleRepository, TypeVehicleRepository>();
        builder.Services.AddScoped<IVehicleHistoryService, VehicleHistoryService>();
        builder.Services.AddScoped<IVehicleHistoryRepository, VehicleHistoryRepository>();
        builder.Services.AddScoped<IVehicleHistoryParkingRatesService, VehicleHistoryParkingRatesService>();
        builder.Services.AddScoped<IVehicleHistoryParkingRatesRepository, VehicleHistoryParkingRatesRepository>();
        builder.Services.AddScoped<IInvoiceService, InvoiceService>();
        builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();

        // 🔹 Agregar servicios de controladores y Swagger
       

        // 🔹 Configurar CORS usando la clase de configuración
        //builder.Services.ConfigureCors(builder.Configuration);

        // 🔹 Configurar logging
        //builder.Logging.AddConsole();

        // 🔹 Construir la aplicación
        var app = builder.Build();

        // 🔹 Aplicar middlewares
        //app.UseConfiguredCors();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors();
        app.UseAuthorization();
        app.MapControllers();

        // 🔹 Ejecutar la aplicación
        app.Run();
