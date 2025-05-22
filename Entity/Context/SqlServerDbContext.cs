using Microsoft.EntityFrameworkCore;

namespace Entity.Context
{
    // Ahora SqlServerDbContext hereda de ApplicationDbContext
    public class SqlServerDbContext : ApplicationDbContext
    {
        public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // ¡IMPORTANTE!
            // Configuraciones específicas de SQL Server si son necesarias.
        }
    }
}