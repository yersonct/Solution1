using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace Data
{
    public class VehicleHistoryParkingRatesData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<VehicleHistoryParkingRatesData> _logger;

        public VehicleHistoryParkingRatesData(ApplicationDbContext context, ILogger<VehicleHistoryParkingRatesData> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region LINQ Methods

        // Obtener todos los registros de VehicleHistoryParkingRates con LINQ
        public async Task<IEnumerable<VehicleHistoryParkingRates>> GetAllAsyncLINQ()
        {
            try
            {
                return await _context.VehicleHistoryParkingRates
                    .Include(vhp => vhp.vehiclehistory)
                    .Include(vhp => vhp.parking)
                    .Include(vhp => vhp.rates)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los registros de VehicleHistoryParkingRates con LINQ.");
                throw;
            }
        }

        // Obtener un registro por ID con LINQ
        public async Task<VehicleHistoryParkingRates?> GetByIdAsyncLINQ(int id)
        {
            try
            {
                return await _context.VehicleHistoryParkingRates
                    .Include(vhp => vhp.vehiclehistory)
                    .Include(vhp => vhp.parking)
                    .Include(vhp => vhp.rates)
                    .FirstOrDefaultAsync(vhp => vhp.id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener VehicleHistoryParkingRates con ID {Id} con LINQ.", id);
                throw;
            }
        }

        // Crear un nuevo registro con LINQ
        public async Task<VehicleHistoryParkingRates> CreateAsyncLINQ(VehicleHistoryParkingRates vehicleHistoryParkingRates)
        {
            try
            {
                await _context.VehicleHistoryParkingRates.AddAsync(vehicleHistoryParkingRates);
                await _context.SaveChangesAsync();
                return vehicleHistoryParkingRates;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo registro de VehicleHistoryParkingRates con LINQ.");
                throw;
            }
        }

        // Actualizar un registro existente con LINQ
        public async Task<bool> UpdateAsyncLINQ(VehicleHistoryParkingRates vehicleHistoryParkingRates)
        {
            try
            {
                _context.VehicleHistoryParkingRates.Update(vehicleHistoryParkingRates);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el registro de VehicleHistoryParkingRates con ID {Id} con LINQ.", vehicleHistoryParkingRates.id);
                return false;
            }
        }

        // Eliminar un registro por ID con LINQ
        public async Task<bool> DeleteAsyncLINQ(int id)
        {
            try
            {
                var vehicleHistoryParkingRates = await _context.VehicleHistoryParkingRates.FindAsync(id);
                if (vehicleHistoryParkingRates == null)
                {
                    return false;
                }

                _context.VehicleHistoryParkingRates.Remove(vehicleHistoryParkingRates);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el registro de VehicleHistoryParkingRates con ID {Id} con LINQ.", id);
                return false;
            }
        }

        #endregion

        #region SQL (Dapper) Methods

        // Obtener todos los registros de VehicleHistoryParkingRates con SQL
        public async Task<IEnumerable<VehicleHistoryParkingRates>> GetAllAsyncSQL()
        {
            try
            {
                using var connection = _context.Database.GetDbConnection();
                connection.Open();
                return await connection.QueryAsync<VehicleHistoryParkingRates>(@"
                    SELECT id AS Id,
                           hourused AS HoursUsed,
                           id_rates AS RatesId,
                           id_vehiclehistory AS VehicleHistoryId,
                           id_parking AS ParkingId,
                           subtotal AS SubTotal
                    FROM vehiclehistoryparkingrates;
                ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los registros de VehicleHistoryParkingRates con SQL.");
                throw;
            }
        }

        // Obtener un registro por ID con SQL
        public async Task<VehicleHistoryParkingRates?> GetByIdAsyncSQL(int id)
        {
            try
            {
                using var connection = _context.Database.GetDbConnection();
                connection.Open();
                return await connection.QueryFirstOrDefaultAsync<VehicleHistoryParkingRates>(@"
                    SELECT id AS Id,
                           hourused AS HoursUsed,
                           id_rates AS RatesId,
                           id_vehiclehistory AS VehicleHistoryId,
                           id_parking AS ParkingId,
                           subtotal AS SubTotal
                    FROM vehiclehistoryparkingrates
                    WHERE id = @Id;", new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener VehicleHistoryParkingRates con ID {Id} con SQL.", id);
                throw;
            }
        }

        // Crear un nuevo registro con SQL
        public async Task<VehicleHistoryParkingRates> CreateAsyncSQL(VehicleHistoryParkingRates vehicleHistoryParkingRates)
        {
            try
            {
                using var connection = _context.Database.GetDbConnection();
                connection.Open();
                var id = await connection.QuerySingleAsync<int>(@"
                    INSERT INTO vehiclehistoryparkingrates (hourused, id_rates, id_vehiclehistory, id_parking, subtotal)
                    VALUES (@HoursUsed, @RatesId, @VehicleHistoryId, @ParkingId, @SubTotal)
                    RETURNING id;", vehicleHistoryParkingRates);
                vehicleHistoryParkingRates.id = id;
                return vehicleHistoryParkingRates;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo registro de VehicleHistoryParkingRates con SQL.");
                throw;
            }
        }

        // Actualizar un registro existente con SQL
        public async Task<bool> UpdateAsyncSQL(VehicleHistoryParkingRates vehicleHistoryParkingRates)
        {
            try
            {
                using var connection = _context.Database.GetDbConnection();
                connection.Open();
                var rowsAffected = await connection.ExecuteAsync(@"
                    UPDATE vehiclehistoryparkingrates
                    SET hourused = @HoursUsed,
                        id_rates = @RatesId,
                        id_vehiclehistory = @VehicleHistoryId,
                        id_parking = @ParkingId,
                        subtotal = @SubTotal
                    WHERE id = @Id;", vehicleHistoryParkingRates);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el registro de VehicleHistoryParkingRates con ID {Id} con SQL.", vehicleHistoryParkingRates.id);
                return false;
            }
        }

        // Eliminar un registro por ID con SQL
        public async Task<bool> DeleteAsyncSQL(int id)
        {
            try
            {
                using var connection = _context.Database.GetDbConnection();
                connection.Open();
                var rowsAffected = await connection.ExecuteAsync(@"
                    DELETE FROM vehiclehistoryparkingrates
                    WHERE id = @Id;", new { Id = id });
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el registro de VehicleHistoryParkingRates con ID {Id} con SQL.", id);
                return false;
            }
        }

        public async Task<IEnumerable<VehicleHistoryParkingRates>> GetByRatesIdAsync(int ratesId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<VehicleHistoryParkingRates>> GetByParkingIdAsync(int parkingId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<VehicleHistoryParkingRates>> GetByVehicleHistoryIdAsync(int vehicleHistoryId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}