// Ruta: Data/Interfaces/IApplicationDbContextWithEntry.cs

// Remueve los using que no son necesarios para una interfaz pura.
// using System; // No se usa directamente aquí
// using System.Collections.Generic; // No se usa directamente aquí
// using System.Linq; // No se usa directamente aquí
// using System.Text; // No se usa directamente aquí
// using System.Threading.Tasks; // No se usa directamente aquí
// using Entity.Model; // No se debería referenciar modelos directamente en la interfaz genérica de contexto.
// using Data.Interfaces; // Esto es redundante si estás en el mismo namespace.

using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure; // Para DatabaseFacade
using System.Threading;
using System.Threading.Tasks;

namespace Data.Interfaces // ¡ESTE ES EL NAMESPACE CORRECTO PARA UNA INTERFAZ EN LA CAPA DATA.INTERFACES!
{
    // Define IApplicationDbContext si no la tienes ya en otro archivo
    // o si necesitas una interfaz base con SaveChangesAsync, Set, Database
    public interface IApplicationDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        public DbSet<User> Users { get; set; }
        public DbSet<Person> Persons { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        DatabaseFacade Database { get; }
    }

    // Esta es tu interfaz principal que hereda de la base
    public interface IApplicationDbContextWithEntry : IApplicationDbContext
    {
        EntityEntry Entry(object entity);

        // Los DbSets NO deben estar aquí.
        // Las interfaces de DbContext solo deben exponer los métodos genéricos
        // (Set<TEntity>, SaveChangesAsync, Entry, Database) para la abstracción.
        // Los DbSets concretos (DbSet<Camara>, DbSet<User>, etc.) van en la clase
        // concreta ApplicationDbContext.
    }
}