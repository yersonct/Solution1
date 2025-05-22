using Microsoft.EntityFrameworkCore;

namespace Entity.Context
{
    // Ahora PostgreSqlDbContext hereda de ApplicationDbContext
    public class PostgreSqlDbContext : ApplicationDbContext
    {
        public PostgreSqlDbContext(DbContextOptions<PostgreSqlDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // ¡IMPORTANTE!
            // Configuraciones específicas de PostgreSQL si son necesarias.
        }
    }
}