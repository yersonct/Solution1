using ANPRVisionAPI.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration; // Necesario para context.Configuration
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

// Importante para Serilog
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Configurar Serilog para logging dinámico en base de datos
// Esto debe ir al principio para que capture logs del inicio de la aplicación
builder.Host.UseSerilog((context, services, configurationBuilder) =>
{
    configurationBuilder
        .ReadFrom.Configuration(context.Configuration) // Lee la configuración base de Serilog desde appsettings.json
        .ReadFrom.Services(services) // Permite a Serilog acceder a servicios (ej. opciones)
        .AddDatabaseLogging(context.Configuration); // Llama a nuestra extensión para añadir el sink de DB
});


// 🔹 Add Controllers
builder.Services.AddControllers();

// 🔹 Configure CORS
builder.Services.AddCorsConfiguration(builder.Configuration);

// 🔹 Swagger Configuration
builder.Services.AddSwaggerConfiguration();

// 🔹 FluentValidation Configuration
builder.Services.AddFluentValidationConfiguration();

// 🔹 Database Context
builder.Services.AddDatabaseContext(builder.Configuration);

// ⚙️ Configure Authentication (Both JWT and OAuth 2.0)
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddOAuth2Authentication(builder.Configuration);

// ⚙️ Configure Authorization Policies
builder.Services.AddApplicationAuthorizationPolicies();

//🔹 Dependency Injection(Services and Repositories)
builder.Services.AddApplicationServices();
builder.Services.AddApplicationRepositories();

var app = builder.Build();

// ⚙️ Use Serilog Request Logging (captura información de las solicitudes HTTP)
app.UseSerilogRequestLogging();

// ⚙️ Configure Environment-Specific Settings
app.ConfigureDevelopmentEnvironment(app.Environment);

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ⚙️ Aplicar Migraciones al iniciar la aplicación (una vez) de forma limpia
await app.Services.UseMigrations();

// ⚙️ Run Application
app.Run();