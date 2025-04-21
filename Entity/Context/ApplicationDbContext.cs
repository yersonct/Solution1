using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Dapper;
using Entity.Model;
using System.Data;

namespace Entity.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<RolUser> RolUsers { get; set; }
        public DbSet<Forms> Forms { get; set; }
        public DbSet<Modules> Modules { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<Rol> Rol { get; set; }
        public DbSet<Camara> Camara { get; set; }
        public DbSet<Parking> Parkings { get; set; }
        public DbSet<MembershipsVehicle> MembershipsVehicles { get; set; }
        public DbSet<TypeRates> TypeRates { get; set; }
        public DbSet<Rates> Rates { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<MemberShips> MemberShips { get; set; }
        public DbSet<FormModule> FormModule { get; set; }
        public DbSet<FormRolPermission> FormRolPermission { get; set; }
        public DbSet<BlackList> BlackList { get; set; }
        public DbSet<Vehicle> Vehicle { get; set; }
        public DbSet<RegisteredVehicle> RegisteredVehicle { get; set; }
        public DbSet<VehicleHistory> VehicleHistory { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<TypeVehicle> TypeVehicle { get; set; }
        public DbSet<VehicleHistoryParkingRates> VehicleHistoryParkingRates { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la relación VehicleHistoryParkingRates y Rates (asegúrate del nombre de la propiedad)
            modelBuilder.Entity<VehicleHistoryParkingRates>()
                .HasOne(vhp => vhp.rates)
                .WithMany(r => r.VehicleHistoryParkingRates)
                .HasForeignKey(vhp => vhp.id_rates)
                .HasConstraintName("fk_rates")
                .OnDelete(DeleteBehavior.Restrict);

            // Definir las tablas para cada entidad
            modelBuilder.Entity<User>().ToTable("user");
            modelBuilder.Entity<Person>().ToTable("person");
            modelBuilder.Entity<Client>().ToTable("client");
            modelBuilder.Entity<Forms>(entity =>
            {
                entity.ToTable("forms");
                entity.Property(e => e.id).HasColumnName("id");
                entity.Property(e => e.name).HasColumnName("name");
                entity.Property(e => e.url).HasColumnName("url");

                // Configuración de la relación con FormModule
                entity.HasMany(f => f.FormModules)
                    .WithOne(fm => fm.Forms)
                    .HasForeignKey(fm => fm.id_forms)
                    .HasConstraintName("FK_Forms_FormModule");
            });
            modelBuilder.Entity<Modules>().ToTable("module");
            modelBuilder.Entity<Permission>(entity => // ¡Configuración de Permission con ThenInclude!
            {
                entity.ToTable("permission");
                entity.HasKey(e => e.id);
                entity.Property(e => e.name).HasColumnName("name");

                entity.HasMany(p => p.FormRolPermissions)
                    .WithOne(frp => frp.Permission)
                    .HasForeignKey(frp => frp.id_permission)
                    .HasConstraintName("FK_FormRolPermission_Permission");
            });
            modelBuilder.Entity<Rol>(entity =>
            {
                entity.ToTable("rol");
                entity.HasKey(e => e.id);

                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.Active).HasColumnName("active");

                // Configuración de la relación con FormRolPermission
                entity.HasMany(r => r.FormRolPermissions)
                    .WithOne(frp => frp.Rol)
                    .HasForeignKey(frp => frp.id_rol)
                    .HasConstraintName("FK_FormRolPermission_Rol");

                // Configuración de la relación con RolUser
                entity.HasMany(r => r.RolUsers)
                    .WithOne(ru => ru.Rol)
                    .HasForeignKey(ru => ru.id_rol)
                    .HasConstraintName("FK_RolUser_Rol");
            });
            modelBuilder.Entity<Camara>().ToTable("camara");
            modelBuilder.Entity<Parking>().ToTable("parking");
            modelBuilder.Entity<MembershipsVehicle>().ToTable("membershipsvehicle");
            modelBuilder.Entity<TypeRates>().ToTable("typerates");
            modelBuilder.Entity<Rates>().ToTable("rates");
            modelBuilder.Entity<MemberShips>().ToTable("memberships");
            modelBuilder.Entity<FormModule>(entity =>
            {
                entity.ToTable("formmodule");
                entity.Property(e => e.id).HasColumnName("id");
                entity.Property(e => e.id_forms).HasColumnName("id_forms");
                entity.Property(e => e.id_module).HasColumnName("id_module");

                entity.HasOne(d => d.Modules)
                    .WithMany(p => p.FormModules)
                    .HasForeignKey(d => d.id_module)
                    .HasConstraintName("FK_FormModule_Modules");

                entity.HasOne(d => d.Forms)
                    .WithMany(p => p.FormModules)
                    .HasForeignKey(d => d.id_forms)
                    .HasConstraintName("FK_FormModule_Forms");
            });
            // INICIO - Configuración de la entidad FormRolPermission
            modelBuilder.Entity<FormRolPermission>(entity =>
            {
                entity.ToTable("formrolpermission");
                entity.HasKey(e => e.id);

                entity.Property(e => e.id).HasColumnName("id");
                entity.Property(e => e.id_forms).HasColumnName("id_forms");
                entity.Property(e => e.id_rol).HasColumnName("id_rol");
                entity.Property(e => e.id_permission).HasColumnName("id_permission");

                // Configuración de la relación con Rol
                entity.HasOne(r => r.Rol)
                    .WithMany(rp => rp.FormRolPermissions)
                    .HasForeignKey(rp => rp.id_rol)
                    .HasConstraintName("FK_FormRolPermission_Rol");

                // Configuración de la relación con Forms
                entity.HasOne(f => f.Forms)
                    .WithMany(fr => fr.FormRolPermissions) // ¡Añadimos la colección de navegación inversa!
                    .HasForeignKey(frp => frp.id_forms)
                    .HasConstraintName("FK_FormRolPermission_Forms");

                // Configuración de la relación con Permission
                entity.HasOne(p => p.Permission)
                    .WithMany(pr => pr.FormRolPermissions)
                    .HasForeignKey(frp => frp.id_permission)
                    .HasConstraintName("FK_FormRolPermission_Permission");
            });
            // FIN - Configuración de la entidad FormRolPermission
            modelBuilder.Entity<BlackList>().ToTable("blacklist");
            modelBuilder.Entity<Vehicle>().ToTable("vehicle");
            modelBuilder.Entity<RegisteredVehicle>().ToTable("registeredvehicle");
            modelBuilder.Entity<VehicleHistory>().ToTable("vehiclehistory");
            modelBuilder.Entity<Invoice>().ToTable("invoice");
            modelBuilder.Entity<TypeVehicle>().ToTable("typevehicles");
            modelBuilder.Entity<VehicleHistoryParkingRates>()
                .ToTable("vehiclehistoryparkingrates");

            // Relaciones de User y Person
            modelBuilder.Entity<User>()
                .HasOne(u => u.person)
                .WithOne(p => p.user)
                .HasForeignKey<User>(u => u.id_person)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación entre Camara y Parking
            modelBuilder.Entity<Parking>()
                .HasOne(p => p.camara)
                .WithOne(c => c.parking)
                .HasForeignKey<Parking>(p => p.id_camara)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación entre Rates y TypeRates
            modelBuilder.Entity<Rates>()
                .HasOne(r => r.TypeRates)
                .WithMany(tr => tr.rates)
                .HasForeignKey(r => r.id_typerates)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación entre MembershipsVehicle y Vehicle
            modelBuilder.Entity<MembershipsVehicle>()
                .HasOne(mv => mv.vehicle)
                .WithMany(v => v.membershipsvehicles)
                .HasForeignKey(mv => mv.id_vehicle)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación entre MembershipsVehicle y Memberships
            modelBuilder.Entity<MembershipsVehicle>()
                .HasOne(mv => mv.memberships)
                .WithMany(m => m.membershipsvehicles)
                .HasForeignKey(mv => mv.id_memberships)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación entre VehicleHistory y Invoice
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.vehiclehistory)
                .WithMany() // Una VehicleHistory puede tener muchas Invoices
                .HasForeignKey(i => i.id_vehiclehistory)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_vehiclehistory");

            // Relación entre VehicleHistory y TypeVehicle
            modelBuilder.Entity<VehicleHistory>()
                .HasOne(vh => vh.typevehicle)
                .WithOne()
                .HasForeignKey<VehicleHistory>(vh => vh.id_typevehicle)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación entre Client y User
            modelBuilder.Entity<Client>()
                .HasOne(c => c.user)
                .WithMany()
                .HasForeignKey(c => c.id_user);

            // Relación entre Vehicle y Client
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.client)
                .WithMany(c => c.vehicles)
                .HasForeignKey(v => v.id_client);

            // Relación entre BlackList y Client
            modelBuilder.Entity<BlackList>()
                .HasOne(b => b.client)
                .WithOne(c => c.blacklist)
                .HasForeignKey<BlackList>(b => b.id_client);

            // Relación entre VehicleHistory y RegisteredVehicle
            modelBuilder.Entity<VehicleHistory>()
                .HasOne(vh => vh.registeredvehicle)
                .WithMany(rv => rv.vehiclehistory)
                .HasForeignKey(vh => vh.id_registeredvehicle);

            // Relación entre RegisteredVehicle y Vehicle
            modelBuilder.Entity<RegisteredVehicle>()
                .HasOne(rv => rv.vehicle)
                .WithMany(v => v.registeredvehicles)
                .HasForeignKey(rv => rv.id_vehicle);

            // Relación entre VehicleHistoryParkingRates y VehicleHistory
            modelBuilder.Entity<VehicleHistoryParkingRates>()
                .HasOne(vhpr => vhpr.vehiclehistory)
                .WithMany(vh => vh.VehicleHistoryParkingRates)
                .HasForeignKey(vhpr => vhpr.id_vehiclehistory)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación entre VehicleHistoryParkingRates y Parking
            modelBuilder.Entity<VehicleHistoryParkingRates>()
                .HasOne(vhpr => vhpr.parking)
                .WithMany(p => p.VehicleHistoryParkingRates)
                .HasForeignKey(vhpr => vhpr.id_parking)
                .OnDelete(DeleteBehavior.Restrict);

            // CONFIGURACIÓN PARA LA ENTIDAD RolUser
            modelBuilder.Entity<RolUser>(entity =>
            {
                entity.ToTable("roluser"); // Usa el nombre de la tabla en minúsculas
                entity.HasKey(e => e.id); // Asegúrate de que RolUser tenga una propiedad 'Id' como clave primaria

                entity.Property(e => e.id_rol).HasColumnName("id_rol");
                entity.Property(e => e.id_user).HasColumnName("id_user");

                // Configuración de la relación con Rol
                entity.HasOne(ru => ru.Rol)
                    .WithMany(r => r.RolUsers)
                    .HasForeignKey(ru => ru.id_rol) // Usa la propiedad id_rol en RolUser
                    .HasConstraintName("FK_RolUser_Rol");

                // Configuración de la relación con User
                entity.HasOne(ru => ru.User)
                    .WithMany(u => u.RolUsers)
                    .HasForeignKey(ru => ru.id_user) // Usa la propiedad id_user en RolUser
                    .HasConstraintName("FK_RolUser_User");
            });

            // CONFIGURACIÓN PARA EL TIPO DE DATO 'time without time zone' PARA VehicleHistory
            modelBuilder.Entity<VehicleHistory>(entity =>
            {
                entity.Property(e => e.totaltime)
                    .HasColumnType("time without time zone");
            });

            // CONFIGURACIÓN PARA EL TIPO DE DATO 'time without time zone' PARA RegisteredVehicle
            modelBuilder.Entity<RegisteredVehicle>(entity =>
            {
                entity.Property(e => e.entrydatetime)
                    .HasColumnType("time without time zone");
                entity.Property(e => e.exitdatetime)
                    .HasColumnType("time without time zone");
            });
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
        }

        public override int SaveChanges()
        {
            EnsureAudit();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            EnsureAudit();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void EnsureAudit()
        {
            ChangeTracker.DetectChanges();
        }

        private async Task<IDbConnection> GetOpenConnectionAsync()
        {
            var connection = Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();
            return connection;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string query, object? parameters = null, int? timeout = null, CommandType? type = null)
        {
            var conn = await GetOpenConnectionAsync();
            return await conn.QueryAsync<T>(query, parameters, commandTimeout: timeout, commandType: type);
        }

        public async Task<T?> QueryFirstOrDefaultAsync<T>(string query, object? parameters = null, int? timeout = null, CommandType? type = null)
        {
            var conn = await GetOpenConnectionAsync();
            return await conn.QueryFirstOrDefaultAsync<T>(query, parameters, commandTimeout: timeout, commandType: type);
        }

        public async Task<T> QuerySingleAsync<T>(string query, object? parameters = null, int? timeout = null, CommandType? type = null)
        {
            var conn = await GetOpenConnectionAsync();
            return await conn.QuerySingleAsync<T>(query, parameters, commandTimeout: timeout, commandType: type);
        }

        // Agregando método ExecuteAsync para operaciones que no devuelven resultados directos
        public async Task<int> ExecuteAsync(string query, object? parameters = null, int? timeout = null, CommandType? type = null)
        {
            var conn = await GetOpenConnectionAsync();
            return await conn.ExecuteAsync(query, parameters, commandTimeout: timeout, commandType: type);
        }
    }
}