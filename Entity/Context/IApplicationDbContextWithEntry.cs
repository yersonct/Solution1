using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Context
{
    public interface IApplicationDbContextWithEntry 
    {
        EntityEntry Entry(object entity);
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
        DbSet<VehicleHistory> VehicleHistories { get; set; }
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        DatabaseFacade Database { get; }
    }
}
