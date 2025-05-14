using ANPRVisionAPI.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;

namespace ANPRVisionAPI.Extensions
{
    public static class AuthenticationCollectionExtensions
    {
        public const string JwtSchemeName = "JwtBearer"; // Define un nombre para el esquema JWT
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {


                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // Esto establecerá el esquema por defecto
                .AddJwtBearer(JwtSchemeName, options => // Especifica el nombre del esquema
                {
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                            logger.LogError(context.Exception, "Autenticación JWT fallida.");
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                            logger.LogInformation("Token JWT validado exitosamente.");
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                            logger.LogWarning("Desafío de autenticación JWT.");
                            return Task.CompletedTask;
                        }
                    };
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                    };
                });
          /* services.AddAuthorization();*/
            return services;
        }


        public static IServiceCollection AddOAuth2Authentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication() // No especificamos el esquema por defecto aquí
                .AddJwtBearer("OAuthBearer", options => // Nombra el esquema OAuth 2.0
                {
                    options.Authority = configuration["OAuth:Authority"];
                    options.Audience = configuration["OAuth:Audience"];

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = configuration["OAuth:Authority"],
                        ValidAudience = configuration["OAuth:Audience"],
                        // La validación de la clave de firma se maneja automáticamente con Authority
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                            logger.LogError(context.Exception, "Autenticación OAuth 2.0 fallida.");
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                            logger.LogInformation("Token OAuth 2.0 validado exitosamente.");
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                            logger.LogWarning("Desafío de autenticación OAuth 2.0.");
                            return Task.CompletedTask;
                        }
                    };
                });
            // No necesitamos AddAuthorization aquí, lo haremos en Program.cs
            return services;
        }

    }

}


/*
[ApiController]
[Route("api/[controller]")]
// Protegido con JWT
[Authorize(AuthenticationSchemes = AuthenticationCollectionExtensions.JwtSchemeName)]
public class JwtProtectedController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Este endpoint está protegido con JWT.");
    }
}

[ApiController]
[Route("api/[controller]")]
// Protegido con OAuth 2.0
[Authorize(AuthenticationSchemes = "OAuthBearer")]
public class OAuthProtectedController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Este endpoint está protegido con OAuth 2.0.");
    }
}

[ApiController]
[Route("api/[controller]")]
// Protegido con cualquiera de los dos esquemas
[Authorize(AuthenticationSchemes = AuthenticationCollectionExtensions.JwtSchemeName + "," + "OAuthBearer")]
public class DualProtectedController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Este endpoint está protegido con JWT o OAuth 2.0.");
    }
}

[ApiController]
[Route("api/[controller]")]
// Protegido con una política específica (que puede usar un esquema u otro)
[Authorize(Policy = "AuthenticatedWithAny")]
public class PolicyProtectedController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Este endpoint está protegido por una política que acepta JWT o OAuth 2.0.");
    }
}
*/