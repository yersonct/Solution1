using Microsoft.EntityFrameworkCore;
// using Data.Interfaces; // Ya no es necesario importar aquí si ApplicationDbContext ya la implementa

namespace Entity.Context
{
    // Ahora MySqlDbContext hereda de ApplicationDbContext
    public class MySqlDbContext : ApplicationDbContext
    {
        public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options) { }

        // NO NECESITAS DUPLICAR LOS DBSETS AQUÍ. YA ESTÁN EN ApplicationDbContext.
        // NO NECESITAS DUPLICAR SaveChangesAsync, Entry, Database. Ya están en ApplicationDbContext.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // ¡IMPORTANTE! Llama a la implementación base
                                                // para que se apliquen todos los mapeos y el HasData.

            // Puedes añadir configuraciones ESPECÍFICAS de MySQL aquí si realmente las necesitas
            // (Ej: configuraciones de tipos de columna muy particulares que no sean genéricas).
            // PERO NO DUPLIQUES mapeo de tablas, columnas o seeding.
        }
    }
}
