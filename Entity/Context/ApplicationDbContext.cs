using Dapper;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

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
        public DbSet<FormRolPermission> FormRolPermissions { get; set; }
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
            modelBuilder.Entity<Forms>().ToTable("forms");
            modelBuilder.Entity<Modules>().ToTable("modules");
            modelBuilder.Entity<Permission>().ToTable("permission");
            modelBuilder.Entity<Rol>().ToTable("rol");
            modelBuilder.Entity<Camara>().ToTable("camara");
            modelBuilder.Entity<Parking>().ToTable("parking");
            modelBuilder.Entity<MembershipsVehicle>().ToTable("membershipsvehicle");
            modelBuilder.Entity<TypeRates>().ToTable("typerates");
            modelBuilder.Entity<Rates>().ToTable("rates");
            modelBuilder.Entity<MemberShips>().ToTable("memberships");
            modelBuilder.Entity<FormModule>().ToTable("formmodule");
            modelBuilder.Entity<FormRolPermission>().ToTable("formrolpermissions");
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
            modelBuilder.Entity<Camara>()
                .HasOne(c => c.parking)
                .WithMany(p => p.Camaras)
                .HasForeignKey(c => c.id_parking)
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
            //modelBuilder.Entity<VehicleHistory>()
            //    .HasOne(vh => vh.Invoice)
            //    .WithOne(i => i.vehiclehistory)
            //    .HasForeignKey<VehicleHistory>(vh => vh.id_invoice)
            //    .OnDelete(DeleteBehavior.Restrict);

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
                .HasOne(b => b.Client)
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

            // CONFIGURACIÓN DE LA RELACIÓN ENTRE INVOICE Y VEHICLEHISTORY
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.vehiclehistory)
                .WithMany() // Una VehicleHistory puede tener muchas Invoices (si es el caso)
                .HasForeignKey(i => i.id_vehiclehistory)
                .IsRequired() // Una Invoice siempre necesita un VehicleHistory
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_vehiclehistory"); // Explicitly name the foreign key constraint

            // CONFIGURACIÓN DE LA RELACIÓN ENTRE VEHICLEHISTORY E INVOICE (OPCIONAL - si quieres la navegación inversa)
            //modelBuilder.Entity<VehicleHistory>()
            //    .HasOne(vh => vh.Invoice)
            //    .WithOne(i => i.vehiclehistory)
            //    .HasForeignKey<VehicleHistory>(vh => vh.id_invoice)
            //    .OnDelete(DeleteBehavior.Restrict)
            //    .HasConstraintName("fk_invoice"); // Explicitly name the foreign key constraint (if applicable)


            // CONFIGURACIÓN PARA EL TIPO DE DATO 'time without time zone' PARA VehicleHistory
            modelBuilder.Entity<VehicleHistory>(entity =>
            {
                entity.Property(e => e.totaltime)
                    .HasColumnType("time without time zone");
                // OPCIONALMENTE:
                // entity.Property(e => e.totaltime)
                //    .HasConversion(v => v.ToString(), v => TimeSpan.Parse(v));
            });

            // CONFIGURACIÓN PARA EL TIPO DE DATO 'time without time zone' PARA RegisteredVehicle
            modelBuilder.Entity<RegisteredVehicle>(entity =>
            {
                entity.Property(e => e.entrydatetime)
                    .HasColumnType("time without time zone");
                // OPCIONALMENTE:
                // entity.Property(e => e.entrydatetime)
                //    .HasConversion(v => v.ToString(), v => TimeSpan.Parse(v));

                entity.Property(e => e.exitdatetime)
                    .HasColumnType("time without time zone");
                // OPCIONALMENTE:
                // entity.Property(e => e.exitdatetime)
                //    .HasConversion(v => v.ToString(), v => TimeSpan.Parse(v));
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