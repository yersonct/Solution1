using Business;
using Business.Interfaces;
using Business.Services;
using Data;
using Data.Interfaces;
using Data.Repository;
using Entity.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Repository;
using FluentValidation.AspNetCore;
using Validators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Add Controllers
builder.Services.AddControllers();

// 🔹 Configure CORS
var origenesPermitidos = builder.Configuration.GetValue<string>("OrigenesPermitidos")?.Split(",");
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// 🔹 Swagger Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ANPR Vision API", Version = "v1" });
});

// 🔹 FluentValidation Configuration
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Validators.RegisterRequestValidator>());

// 🔹 PostgreSQL Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔹 Dependency Injection (Services and Repositories)
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
builder.Services.AddScoped<IMembershipsVehicleService, MembershipsVehicleService>();
builder.Services.AddScoped<IMembershipsVehicleRepository, MembershipsVehicleRepository>();
builder.Services.AddScoped<ITypeVehicleService, TypeVehicleService>();
builder.Services.AddScoped<ITypeVehicleRepository, TypeVehicleRepository>();
builder.Services.AddScoped<IVehicleHistoryService, VehicleHistoryService>();
builder.Services.AddScoped<IVehicleHistoryRepository, VehicleHistoryRepository>();
builder.Services.AddScoped<IVehicleHistoryParkingRatesService, VehicleHistoryParkingRatesService>();
builder.Services.AddScoped<IVehicleHistoryParkingRatesRepository, VehicleHistoryParkingRatesRepository>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IloginRepository, UserRepository>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();

// ⚙️ Register ILoggerFactory (ILogger<> is already handled by .NET)
builder.Services.AddSingleton<ILoggerFactory, LoggerFactory>();
// builder.Services.AddScoped(typeof(ILogger<>), typeof(Logger<>));

var app = builder.Build();

// ⚙️ Perform Password Migration on Startup (Development Environment)
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var authService = services.GetRequiredService<IAuthService>();
            var logger = services.GetRequiredService<ILogger<Program>>();

            await authService.MigratePasswordsAsync();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Ocurrió un error durante la migración de contraseñas al inicio.");
        }
    }

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// ⚙️ Run Application
app.Run();