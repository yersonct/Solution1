using Entity.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Data.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Camara> Camara { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Person> Persons { get; set; }
        DbSet<RolUser> RolUsers { get; set; }
        DbSet<Forms> Forms { get; set; }
        DbSet<Modules> Modules { get; set; }
        DbSet<Permission> Permission { get; set; }
        DbSet<Rol> Rol { get; set; }
        DbSet<FormModule> FormModule { get; set; }
        DbSet<FormRolPermission> FormRolPermission { get; set; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        DatabaseFacade Database { get; }
    }
}