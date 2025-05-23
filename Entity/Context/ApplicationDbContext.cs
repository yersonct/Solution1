// Entity/Context/ApplicationDbContext.cs

using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Data.Interfaces; // Asegúrate de que este namespace sea correcto para tu IApplicationDbContextWithEntry

namespace Entity.Context
{
    public class ApplicationDbContext : DbContext, IApplicationDbContextWithEntry
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        // ***** DBSETS *****
        public DbSet<User> Users { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<RolUser> RolUsers { get; set; }
        public DbSet<Forms> Forms { get; set; }
        public DbSet<Modules> Modules { get; set; } // Nombre de DbSet coincide con la clase Modules
        public DbSet<Permission> Permissions { get; set; } // Cambié a plural para consistencia con otros DbSets
        public DbSet<Rol> Rols { get; set; } // Cambié a plural para consistencia con otros DbSets
        public DbSet<FormModule> FormModules { get; set; }
        public DbSet<FormRolPermission> FormRolPermissions { get; set; }

        // ***** IMPLEMENTACIONES DE MÉTODOS DE INTERFAZ (Si IApplicationDbContextWithEntry los requiere) *****
        public DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        public DatabaseFacade Database => base.Database;

        public EntityEntry Entry(object entity)
        {
            return base.Entry(entity);
        }

        // ***** LÓGICA DE CONFIGURACIÓN DEL MODELO Y SEEDING DE DATOS *****
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // ¡Siempre llama a la implementación base primero!

            // --- Configuraciones de Relaciones ---

            // Configuración de User y Person (relación uno-a-uno)
            // La clave foránea 'PersonId' está en la tabla 'User'.
            // Un usuario tiene una persona, y una persona tiene un usuario.
            modelBuilder.Entity<User>()
                .HasOne(u => u.Person) // Un User tiene una Person
                .WithOne(p => p.User) // Una Person tiene un User
                .HasForeignKey<User>(u => u.PersonId) // La FK está en la entidad User y apunta a Person
                .IsRequired() // Un usuario siempre debe estar asociado a una persona
                .OnDelete(DeleteBehavior.Cascade); // Si se elimina una persona, se elimina el usuario asociado (¡Cuidado con esto!)
                                                   // Puedes cambiar a .OnDelete(DeleteBehavior.Restrict); si no quieres cascada


            // Configuración de RolUser (relación muchos-a-muchos entre User y Rol)
            // RolUser tiene su propia clave primaria 'Id', por lo que no es una tabla de unión pura.
            modelBuilder.Entity<RolUser>()
                .HasOne(ru => ru.User)
                .WithMany(u => u.RolUsers)
                .HasForeignKey(ru => ru.UserId) // Usa UserId, que es el nombre PascalCase de la propiedad en RolUser
                .OnDelete(DeleteBehavior.Restrict); // Evita eliminaciones en cascada

            modelBuilder.Entity<RolUser>()
                .HasOne(ru => ru.Rol)
                .WithMany(r => r.RolUsers)
                .HasForeignKey(ru => ru.RolId) // Usa RolId, que es el nombre PascalCase de la propiedad en RolUser
                .OnDelete(DeleteBehavior.Restrict);


            // Configuración de FormModule (relación muchos-a-muchos entre Forms y Modules)
            modelBuilder.Entity<FormModule>()
                .HasOne(fm => fm.Forms)
                .WithMany(f => f.FormModules)
                .HasForeignKey(fm => fm.FormId) // Usa FormId
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FormModule>()
                .HasOne(fm => fm.Modules)
                .WithMany(m => m.FormModules)
                .HasForeignKey(fm => fm.ModuleId) // Usa ModuleId
                .OnDelete(DeleteBehavior.Restrict);


            // Configuración de FormRolPermission (relación muchos-a-muchos entre Forms, Rol y Permission)
            modelBuilder.Entity<FormRolPermission>()
                .HasOne(frp => frp.Rol)
                .WithMany(r => r.FormRolPermissions)
                .HasForeignKey(frp => frp.RolId) // Usa RolId
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FormRolPermission>()
                .HasOne(frp => frp.Forms)
                .WithMany(f => f.FormRolPermissions)
                .HasForeignKey(frp => frp.FormId) // Usa FormId
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FormRolPermission>()
                .HasOne(frp => frp.Permission)
                .WithMany(p => p.FormRolPermissions)
                .HasForeignKey(frp => frp.PermissionId) // Usa PermissionId
                .OnDelete(DeleteBehavior.Restrict);

            // --- Configuraciones adicionales de propiedades y unicidad ---
            // Aquí puedes añadir índices únicos, longitudes máximas si no las pusiste en los Data Annotations, etc.

            // User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
            // Si el correo electrónico en Person también debe ser único a nivel de BD
            modelBuilder.Entity<Person>()
                .HasIndex(p => p.Email)
                .IsUnique();
            modelBuilder.Entity<Person>()
                .HasIndex(p => p.Document)
                .IsUnique();

            // Rol (Ya tienes las anotaciones [Required] y [StringLength] en el modelo, lo cual es preferible)
            // modelBuilder.Entity<Rol>().Property(r => r.Name).IsRequired().HasMaxLength(50);

            // Permission (Añadido)
            modelBuilder.Entity<Permission>()
                .ToTable("Permissions") // O "Permission" si así está en tu BD y tu [Table] es "Permission"
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50); // Ajusta la longitud si es necesario

            // ***** INICIALIZACIÓN DE DATOS (SEEDING) CON HASDATA *****
            // Asegúrate de que los nombres de las propiedades en HasData coincidan con PascalCase si así está tu modelo
            modelBuilder.Entity<Rol>().HasData(
                new Rol { Id = 1, Name = "Administrador", Description = "Rol con acceso completo al sistema", Active = true },
                new Rol { Id = 2, Name = "Usuario", Description = "Rol con permisos estándar", Active = true },
                new Rol { Id = 3, Name = "Invitado", Description = "Rol con acceso limitado", Active = true }
            );

            // Seed de datos para Permission (ejemplo)
            modelBuilder.Entity<Permission>().HasData(
                new Permission { Id = 1, Name = "Crear", Active = true },
                new Permission { Id = 2, Name = "Leer", Active = true },
                new Permission { Id = 3, Name = "Actualizar", Active = true },
                new Permission { Id = 4, Name = "Eliminar", Active = true }
            );
        }
    }
}