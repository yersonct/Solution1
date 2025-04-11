using System;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class VehicleData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<VehicleData> _logger;

        public VehicleData(ApplicationDbContext context, ILogger<VehicleData> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Método auxiliar para verificar la conexión con la base de datos
        public async Task<bool> CanConnectAsync()
        {
            try
            {
                return await _context.Database.CanConnectAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar la conexión a la base de datos.");
                return false;
            }
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsyncSQL()
        {
            string query = @"SELECT id, plate, color, id_client FROM vehicle;";
            return await _context.QueryAsync<Vehicle>(query);
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            return await _context.Set<Vehicle>()
                                 .Include(v => v.client)
                                 .ToListAsync();
        }

        public async Task<Vehicle?> GetByIdAsyncSQL(int id)
        {
            try
            {
                string query = @"SELECT id, plate, color, id_client FROM vehicle WHERE id = @id;";
                return await _context.QueryFirstOrDefaultAsync<Vehicle>(query, new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el vehículo con ID {VehicleId}", id);
                throw;
            }
        }

        public async Task<Vehicle?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Vehicle>()
                                     .Include(v => v.client)
                                     .FirstOrDefaultAsync(v => v.id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el vehículo con ID {VehicleId}", id);
                throw;
            }
        }

        public async Task<Vehicle> CreateAsyncSQL(Vehicle vehicle)
        {
            try
            {
                string query = @"
                    INSERT INTO vehicle (plate, color, id_client)
                    VALUES (@plate, @color, @id_client)
                    RETURNING id;";

                vehicle.id = await _context.QuerySingleAsync<int>(query, new
                {
                    vehicle.plate,
                    vehicle.color,
                    vehicle.id_client
                });

                return vehicle;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el vehículo: {ex.Message}");
                throw;
            }
        }

        public async Task<Vehicle> CreateAsync(Vehicle vehicle)
        {
            try
            {
                await _context.Set<Vehicle>().AddAsync(vehicle);
                await _context.SaveChangesAsync();
                return vehicle;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el vehículo: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsyncSQL(Vehicle vehicle)
        {
            try
            {
                string query = @"
                    UPDATE vehicle
                    SET plate = @plate, color = @color, id_client = @id_client
                    WHERE id = @id;
                    SELECT 1;";

                int rowsAffected = await _context.QuerySingleAsync<int>(query, new
                {
                    vehicle.id,
                    vehicle.plate,
                    vehicle.color,
                    vehicle.id_client
                });

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el vehículo: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Vehicle vehicle)
        {
            try
            {
                _context.Set<Vehicle>().Update(vehicle);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el vehículo: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsyncSQL(int id)
        {
            try
            {
                string query = "DELETE FROM vehicle WHERE id = @id RETURNING 1;";
                int rowsAffected = await _context.QuerySingleAsync<int>(query, new { id });
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el vehículo: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var vehicle = await GetByIdAsync(id);
                if (vehicle == null)
                    return false;

                _context.Vehicle.Remove(vehicle);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el vehículo: {ex.Message}");
                return false;
            }
        }
    }
}
