using ANPRVisionAPI.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Business.Interfaces;

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

// ⚙️ Configure Authentication (Both JWT and OAuth 2.0)
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddOAuth2Authentication(builder.Configuration);

// ⚙️ Configure Authorization Policies
builder.Services.AddApplicationAuthorizationPolicies();

//🔹 Dependency Injection(Services and Repositories)
builder.Services.AddApplicationServices();
builder.Services.AddApplicationRepositories();

// ⚙️ Register ILoggerFactory (ILogger<> is already handled by .NET)
builder.Services.AddSingleton<ILoggerFactory, LoggerFactory>();

var app = builder.Build();

// ⚙️ Configure Environment-Specific Settings
app.ConfigureDevelopmentEnvironment(app.Environment);

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ⚙️ Run Application
app.Run();