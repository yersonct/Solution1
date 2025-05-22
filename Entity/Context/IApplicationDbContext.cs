// Data/Interfaces/IApplicationDbContextWithEntry.cs (Ejemplo, verifica tu archivo real)
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking; // Para EntityEntry
using Microsoft.EntityFrameworkCore.Infrastructure; // Para DatabaseFacade

namespace Data.Interfaces // Asegúrate de que el namespace sea correcto
{
    public interface IApplicationDbContext // Esta sería la interfaz sin Entry
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        DatabaseFacade Database { get; }
        // Agrega aquí cualquier otro DbSet que quieras exponer a través de la interfaz
        // Por ejemplo:
        // DbSet<Rol> Rol { get; }
    }
}