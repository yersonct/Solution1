// Entity/Context/ApplicationDbContext.cs

using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Data.Interfaces; // ESTE USING ES VITAL PARA LA INTERFAZ

namespace Entity.Context
{
    public class ApplicationDbContext : DbContext, IApplicationDbContextWithEntry
    {
        // Ambos constructores son válidos para tu escenario
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        // ***** DBSETS *****
        public DbSet<Camara> Camara { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<RolUser> RolUsers { get; set; }
        public DbSet<Forms> Forms { get; set; }
        public DbSet<Modules> Modules { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<Rol> Rol { get; set; }
        public DbSet<FormModule> FormModule { get; set; }
        public DbSet<FormRolPermission> FormRolPermission { get; set; }
        public DbSet<VehicleHistory> VehicleHistories { get; set; }

        // ***** IMPLEMENTACIONES DE MÉTODOS DE INTERFAZ *****
        public DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        public DatabaseFacade Database => base.Database;

        // Implementación de Entry para IApplicationDbContextWithEntry
        public EntityEntry Entry(object entity)
        {
            return base.Entry(entity);
        }

        // ***** LÓGICA DE CONFIGURACIÓN DEL MODELO Y SEEDING DE DATOS *****
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // ¡Siempre llama a la implementación base!

            // --- Mapeo de Tablas ---
            modelBuilder.Entity<Camara>().ToTable("camara");
            modelBuilder.Entity<User>().ToTable("user");
            modelBuilder.Entity<Person>().ToTable("person");
            modelBuilder.Entity<Rol>().ToTable("rol");
            modelBuilder.Entity<Forms>().ToTable("forms");
            modelBuilder.Entity<Modules>().ToTable("module");
            modelBuilder.Entity<Permission>().ToTable("permission");
            modelBuilder.Entity<RolUser>().ToTable("roluser");
            modelBuilder.Entity<FormModule>().ToTable("formmodule");
            modelBuilder.Entity<FormRolPermission>().ToTable("formrolpermission");
            modelBuilder.Entity<VehicleHistory>().ToTable("vehiclehistory");

            // --- Mapeo de Columnas (Verifica la consistencia con tu clase Rol) ---
            modelBuilder.Entity<User>().Property(e => e.id).HasColumnName("id");
            modelBuilder.Entity<Person>().Property(e => e.id).HasColumnName("id");
            modelBuilder.Entity<Rol>().Property(e => e.id).HasColumnName("id");
            modelBuilder.Entity<Forms>().Property(e => e.id).HasColumnName("id");
            modelBuilder.Entity<Modules>().Property(e => e.id).HasColumnName("id");
            modelBuilder.Entity<Permission>().Property(e => e.id).HasColumnName("id");
            modelBuilder.Entity<RolUser>().Property(e => e.id).HasColumnName("id");
            modelBuilder.Entity<FormModule>().Property(e => e.id).HasColumnName("id");
            modelBuilder.Entity<FormRolPermission>().Property(e => e.id).HasColumnName("id");

            modelBuilder.Entity<Rol>().Property(e => e.name).HasColumnName("name");
            modelBuilder.Entity<Rol>().Property(e => e.description).HasColumnName("description");
            modelBuilder.Entity<Rol>().Property(e => e.active).HasColumnName("active");
            modelBuilder.Entity<Forms>().Property(e => e.name).HasColumnName("name");
            modelBuilder.Entity<Forms>().Property(e => e.url).HasColumnName("url");
            modelBuilder.Entity<Modules>().Property(e => e.name).HasColumnName("name");
            modelBuilder.Entity<Permission>().Property(e => e.name).HasColumnName("name");

            // --- Configuraciones de Relaciones ---
            modelBuilder.Entity<User>()
                .HasOne(u => u.person)
                .WithOne(p => p.user)
                .HasForeignKey<User>(u => u.id_person)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<RolUser>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.HasOne(ru => ru.Rol)
                    .WithMany(r => r.RolUsers)
                    .HasForeignKey(ru => ru.id_rol)
                    .HasConstraintName("FK_RolUser_Rol");
                entity.HasOne(ru => ru.User)
                    .WithMany(u => u.RolUsers)
                    .HasForeignKey(ru => ru.id_user)
                    .HasConstraintName("FK_RolUser_User");
            });
            modelBuilder.Entity<FormModule>(entity =>
            {
                entity.HasOne(fm => fm.Forms)
                    .WithMany(f => f.FormModules)
                    .HasForeignKey(fm => fm.id_forms)
                    .HasConstraintName("FK_FormModule_Forms");
                entity.HasOne(fm => fm.Modules)
                    .WithMany(m => m.FormModules)
                    .HasForeignKey(fm => fm.id_module)
                    .HasConstraintName("FK_FormModule_Modules");
            });
            modelBuilder.Entity<FormRolPermission>(entity =>
            {
                entity.HasOne(frp => frp.Rol)
                    .WithMany(r => r.FormRolPermissions)
                    .HasForeignKey(frp => frp.id_rol)
                    .HasConstraintName("FK_FormRolPermission_Rol");
                entity.HasOne(frp => frp.Forms)
                    .WithMany(f => f.FormRolPermissions)
                    .HasForeignKey(frp => frp.id_forms)
                    .HasConstraintName("FK_FormRolPermission_Forms");
                entity.HasOne(frp => frp.Permission)
                    .WithMany(p => p.FormRolPermissions)
                    .HasForeignKey(frp => frp.id_permission)
                    .HasConstraintName("FK_FormRolPermission_Permission");
            });

            // ***** INICIALIZACIÓN DE DATOS (SEEDING) CON HASDATA *****
            modelBuilder.Entity<Rol>().HasData(
                new Rol { id = 1, name = "Administrador", description = "Rol con acceso completo al sistema", active = true },
                new Rol { id = 2, name = "Usuario", description = "Rol con permisos estándar", active = true },
                new Rol { id = 3, name = "Invitado", description = "Rol con acceso limitado", active = true }
            );
        }
    }
}