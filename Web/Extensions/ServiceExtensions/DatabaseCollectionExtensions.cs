// ANPRVisionAPI.Extensions/DatabaseCollectionExtensions.cs
using Data.Interfaces; // Asegúrate de que esta interfaz exista y contenga lo que esperas
using Entity.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ANPRVisionAPI.Extensions
{
    public static class DatabaseCollectionExtensions
    {
        public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseType = configuration.GetSection("DatabaseSettings")["DatabaseType"]?.ToLower();
            string connectionString;

            switch (databaseType)
            {
                case "mysql":
                    connectionString = configuration.GetConnectionString("MySqlConnection");
                    services.AddDbContext<MySqlDbContext>(options =>
                        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
                    // Registra MySqlDbContext como la implementación de IApplicationDbContextWithEntry
                    services.AddScoped<IApplicationDbContextWithEntry, MySqlDbContext>();
                    break;
                case "postgresql":
                    connectionString = configuration.GetConnectionString("LocalPostgres");
                    services.AddDbContext<PostgreSqlDbContext>(options =>
                        options.UseNpgsql(connectionString));
                    // Registra PostgreSqlDbContext como la implementación de IApplicationDbContextWithEntry
                    services.AddScoped<IApplicationDbContextWithEntry, PostgreSqlDbContext>();
                    break;
                case "sqlserver":
                    connectionString = configuration.GetConnectionString("SqlServerConnection");
                    services.AddDbContext<SqlServerDbContext>(options =>
                        options.UseSqlServer(connectionString));
                    // Registra SqlServerDbContext como la implementación de IApplicationDbContextWithEntry
                    services.AddScoped<IApplicationDbContextWithEntry, SqlServerDbContext>();
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported database type: {databaseType}");
            }

            return services;
        }

        public static async Task UseMigrations(this IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    // CAMBIO CLAVE: Se resuelve IApplicationDbContextWithEntry, que es la interfaz que tus DbContexts específicos implementan y que estás registrando.
                    var context = services.GetRequiredService<IApplicationDbContextWithEntry>() as DbContext;

                    if (context != null)
                    {
                        await context.Database.MigrateAsync(); // Usar MigrateAsync para ser asíncrono
                        Console.WriteLine("Migraciones aplicadas exitosamente.");
                    }
                    else
                    {
                        Console.WriteLine("Error: No se pudo resolver la implementación de IApplicationDbContextWithEntry para la migración.");
                    }
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Ocurrió un error al migrar o inicializar la base de datos.");
                }
            }
        }
    }
}