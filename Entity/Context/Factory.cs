using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Context
{
    public class MySqlDbContextFactory : IDbContextFactory
    {
        private readonly IConfiguration _configuration;

        public MySqlDbContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbContext CreateDbContext()
        {
            var connectionString = _configuration.GetConnectionString("MySqlConnection");
            var optionsBuilder = new DbContextOptionsBuilder<MySqlDbContext>();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            return new MySqlDbContext(optionsBuilder.Options);
        }
    }

    public class PostgreSqlDbContextFactory : IDbContextFactory
    {
        private readonly IConfiguration _configuration;

        public PostgreSqlDbContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbContext CreateDbContext()
        {
            var connectionString = _configuration.GetConnectionString("LocalPostgres");
            var optionsBuilder = new DbContextOptionsBuilder<PostgreSqlDbContext>();
            optionsBuilder.UseNpgsql(connectionString);
            return new PostgreSqlDbContext(optionsBuilder.Options);
        }
    }

    public class SqlServerDbContextFactory : IDbContextFactory
    {
        private readonly IConfiguration _configuration;

        public SqlServerDbContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbContext CreateDbContext()
        {
            var connectionString = _configuration.GetConnectionString("SqlServerConnection");
            var optionsBuilder = new DbContextOptionsBuilder<SqlServerDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new SqlServerDbContext(optionsBuilder.Options);
        }
    }
}
