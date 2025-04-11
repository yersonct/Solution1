using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class RegisteredVehicleData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RegisteredVehicleData> _logger;

        public RegisteredVehicleData(ApplicationDbContext context, ILogger<RegisteredVehicleData> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> CanConnectAsync()
        {
            try
            {
                return await _context.Database.CanConnectAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar conexión a la base de datos.");
                return false;
            }
        }

        // ----------- Métodos con Dapper -----------

        public async Task<IEnumerable<RegisteredVehicle>> GetAllAsyncSQL()
        {
            string query = @"SELECT id, entrydatetime, exitdatetime, id_vehicle FROM registeredvehicle";
            return await _context.QueryAsync<RegisteredVehicle>(query);
        }

        public async Task<RegisteredVehicle?> GetByIdAsyncSQL(int id)
        {
            try
            {
                string query = @"SELECT id, entrydatetime, exitdatetime, id_vehicle FROM registeredvehicle WHERE id = @id";
                return await _context.QueryFirstOrDefaultAsync<RegisteredVehicle>(query, new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener RegisteredVehicle con ID {Id}", id);
                throw;
            }
        }

        public async Task<RegisteredVehicle> CreateAsyncSQL(RegisteredVehicle rv)
        {
            try
            {
                string query = @"
                    INSERT INTO registeredvehicle (entrydatetime, exitdatetime, id_vehicle)
                    VALUES (@entrydatetime, @exitdatetime, @id_vehicle)
                    RETURNING id;
                ";

                rv.id = await _context.QuerySingleAsync<int>(query, new
                {
                    rv.entrydatetime,
                    rv.exitdatetime,
                    rv.id_vehicle
                });

                return rv;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear RegisteredVehicle.");
                throw;
            }
        }

        public async Task<bool> UpdateAsyncSQL(RegisteredVehicle rv)
        {
            try
            {
                string query = @"
                    UPDATE registeredvehicle
                    SET entrydatetime = @entrydatetime,
                        exitdatetime = @exitdatetime,
                        id_vehicle = @id_vehicle
                    WHERE id = @id;
                    SELECT 1;
                ";

                int result = await _context.QuerySingleAsync<int>(query, new
                {
                    rv.id,
                    rv.entrydatetime,
                    rv.exitdatetime,
                    rv.id_vehicle
                });

                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar RegisteredVehicle con ID {Id}", rv.id);
                return false;
            }
        }

        public async Task<bool> DeleteAsyncSQL(int id)
        {
            try
            {
                string query = @"DELETE FROM registeredvehicle WHERE id = @id RETURNING 1;";
                int result = await _context.QuerySingleAsync<int>(query, new { id });
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar RegisteredVehicle con ID {Id}", id);
                return false;
            }
        }


        public async Task<IEnumerable<RegisteredVehicle>> GetAllAsync()
        {
            try
            {
                return await _context.Set<RegisteredVehicle>()
                    .Include(rv => rv.vehicle) // Asegúrate de incluir la navegación a Vehicle si la necesitas
                    .Include(rv => rv.vehiclehistory) // Asegúrate de incluir la navegación a VehicleHistory si la necesitas
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener registros de vehículos.");
                throw;
            }
        }

        public async Task<RegisteredVehicle?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<RegisteredVehicle>()
                    .Include(rv => rv.vehicle)
                    .Include(rv => rv.vehiclehistory)
                    .FirstOrDefaultAsync(rv => rv.id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener RegisteredVehicle con ID {Id}", id);
                throw;
            }
        }

        public async Task<RegisteredVehicle> CreateAsync(RegisteredVehicle rv)
        {
            try
            {
                await _context.Set<RegisteredVehicle>().AddAsync(rv);
                await _context.SaveChangesAsync();
                return rv;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear RegisteredVehicle.");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(RegisteredVehicle rv)
        {
            try
            {
                _context.Set<RegisteredVehicle>().Update(rv);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar RegisteredVehicle con ID {Id}", rv.id);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var rv = await GetByIdAsync(id);
                if (rv == null)
                    return false;

                _context.Set<RegisteredVehicle>().Remove(rv);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar RegisteredVehicle con ID {Id}", id);
                return false;
            }
        }
    }
}