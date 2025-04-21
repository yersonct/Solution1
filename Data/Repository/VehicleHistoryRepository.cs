using Data.Interfaces;
using Entity.Context;
using Entity.DTOs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class VehicleHistoryRepository : IVehicleHistoryRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly ILogger<VehicleHistoryRepository> _logger;

        public VehicleHistoryRepository(ApplicationDbContext context, ILogger<VehicleHistoryRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> AddAsync(VehicleHistoryCreateDTO entity)
        {
            try
            {
                string query = @"INSERT INTO vehiclehistory (totaltime, id_registeredvehicle, id_typevehicle)
                                VALUES (@TotalTime, @RegisteredVehicleId, @TypeVehicleId)
                                RETURNING id;";
                return await _context.QuerySingleAsync<int>(query, entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar el historial del vehículo.");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                string query = @"DELETE FROM vehiclehistory WHERE id = @Id; SELECT changes() > 0;";
                return await _context.QuerySingleAsync<bool>(query, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el historial del vehículo con ID: {VehicleHistoryId}", id);
                return false;
            }
        }

        public async Task<IEnumerable<VehicleHistoryDTO>> GetAllAsync()
        {
            try
            {
                string query = @"SELECT id, totaltime, id_registeredvehicle, id_typevehicle FROM vehiclehistory;";
                return await _context.QueryAsync<VehicleHistoryDTO>(query);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los historiales de vehículos.");
                return new List<VehicleHistoryDTO>();
            }
        }

        public async Task<VehicleHistoryDTO?> GetByIdAsync(int id)
        {
            try
            {
                string query = @"SELECT id, totaltime, id_registeredvehicle, id_typevehicle FROM vehiclehistory WHERE id = @Id;";
                return await _context.QueryFirstOrDefaultAsync<VehicleHistoryDTO>(query, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el historial del vehículo con ID: {VehicleHistoryId}", id);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(int id, VehicleHistoryCreateDTO entity)
        {
            try
            {
                string query = @"UPDATE vehiclehistory
                                SET totaltime = @TotalTime,
                                    id_registeredvehicle = @RegisteredVehicleId,
                                    id_typevehicle = @TypeVehicleId
                                WHERE id = @Id;
                                SELECT changes() > 0;";
                return await _context.QuerySingleAsync<bool>(query, new { Id = id, entity.TotalTime, entity.RegisteredVehicleId, entity.TypeVehicleId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el historial del vehículo con ID: {VehicleHistoryId}", id);
                return false;
            }
        }
    }
}
