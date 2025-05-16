using Data.Interfaces;
using Entity.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ANPRVisionAPI.Extensions
{
    public static class DatabaseCollectionExtensions
    {
        public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            // Determina el tipo de base de datos desde la configuración
            var databaseType = configuration.GetSection("DatabaseSettings")["DatabaseType"]?.ToLower();

            // Registra la fábrica de DbContext
            services.AddScoped<IDbContextFactory>(provider =>
            {
                switch (databaseType)
                {
                    case "mysql":
                        return new MySqlDbContextFactory(configuration);
                    case "postgresql":
                        return new PostgreSqlDbContextFactory(configuration);
                    case "sqlserver":
                        return new SqlServerDbContextFactory(configuration);
                    default:
                        throw new ArgumentException($"Unsupported database type: {databaseType}");
                }
            });

            // Registra los DbContext con un alcance más corto (Transient)
            // La fábrica se encargará de crear las instancias.
            services.AddTransient(provider => provider.GetRequiredService<IDbContextFactory>().CreateDbContext());

            // Registra las interfaces IApplicationDbContext e IApplicationDbContextWithEntry
            // para que usen el DbContext creado por la fábrica.  Esto es CRUCIAL.
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<DbContext>() as IApplicationDbContext);
            services.AddScoped<IApplicationDbContextWithEntry>(provider => provider.GetRequiredService<DbContext>() as IApplicationDbContextWithEntry);

            return services;
        }
    }
}