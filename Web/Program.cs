using ANPRVisionAPI.Extensions;
using Business.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Add Controllers
builder.Services.AddControllers();

// 🔹 Configure CORS
builder.Services.AddCorsConfiguration(builder.Configuration);

// 🔹 Swagger Configuration
builder.Services.AddSwaggerConfiguration();

// 🔹 FluentValidation Configuration
builder.Services.AddFluentValidationConfiguration();

// 🔹 PostgreSQL Database Context
builder.Services.AddDatabaseContext(builder.Configuration);

// ⚙️ Configure JWT Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

//🔹 Dependency Injection(Services and Repositories)
builder.Services.AddApplicationServices();
builder.Services.AddApplicationRepositories();

// ⚙️ Register ILoggerFactory (ILogger<> is already handled by .NET)
builder.Services.AddSingleton<ILoggerFactory, LoggerFactory>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ⚙️ Run Application
app.Run();