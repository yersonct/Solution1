using Entity.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Entity.Context
{
    public class MySqlDbContext : DbContext, IApplicationDbContextWithEntry
    {
        public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options) { }

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


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        public EntityEntry Entry(object entity)
        {
            return base.Entry(entity);
        }
        public DatabaseFacade Database => base.Database;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("user");
            modelBuilder.Entity<Person>().ToTable("person");
            modelBuilder.Entity<Rol>().ToTable("rol");
            modelBuilder.Entity<Forms>().ToTable("forms");
            modelBuilder.Entity<Modules>().ToTable("module");
            modelBuilder.Entity<Permission>().ToTable("permission");
            modelBuilder.Entity<RolUser>().ToTable("roluser");
            modelBuilder.Entity<FormModule>().ToTable("formmodule");
            modelBuilder.Entity<FormRolPermission>().ToTable("formrolpermission");
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
            modelBuilder.Entity<User>().Property(e => e.id).HasColumnName("id");
            modelBuilder.Entity<Person>().Property(e => e.id).HasColumnName("id");
            modelBuilder.Entity<Rol>().Property(e => e.id).HasColumnName("id");
            modelBuilder.Entity<Forms>().Property(e => e.id).HasColumnName("id");
            modelBuilder.Entity<Modules>().Property(e => e.id).HasColumnName("id");
            modelBuilder.Entity<Permission>().Property(e => e.id).HasColumnName("id");
            modelBuilder.Entity<RolUser>().Property(e => e.id).HasColumnName("id");
            modelBuilder.Entity<FormModule>().Property(e => e.id).HasColumnName("id");
            modelBuilder.Entity<FormRolPermission>().Property(e => e.id).HasColumnName("id");
            modelBuilder.Entity<Rol>().Property(e => e.Name).HasColumnName("name");
            modelBuilder.Entity<Rol>().Property(e => e.Description).HasColumnName("description");
            modelBuilder.Entity<Rol>().Property(e => e.Active).HasColumnName("active");
            modelBuilder.Entity<Forms>().Property(e => e.name).HasColumnName("name");
            modelBuilder.Entity<Forms>().Property(e => e.url).HasColumnName("url");
            modelBuilder.Entity<Modules>().Property(e => e.name).HasColumnName("name");
            modelBuilder.Entity<Permission>().Property(e => e.name).HasColumnName("name");
        }
    }
}