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
            services.AddDbContext<PostgreSqlDbContext>(options =>  options.UseNpgsql(configuration.GetConnectionString("LocalPostgres")));
            services.AddDbContext<MySqlDbContext>(options => options.UseMySql(configuration.GetConnectionString("MySqlConnection"), ServerVersion.AutoDetect(configuration.GetConnectionString("MySqlConnection"))));
            services.AddDbContext<SqlServerDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlServerConnection")));
            return services;
        }
    }
}