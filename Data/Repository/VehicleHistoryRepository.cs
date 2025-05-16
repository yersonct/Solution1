using Data.Interfaces;
using Entity.Context; // Asegúrate de que esta línea es correcta
using Entity.DTOs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class VehicleHistoryRepository : IVehicleHistoryRepository
    {
        protected readonly IApplicationDbContext _context;
        private readonly ILogger<VehicleHistoryRepository> _logger;

        public VehicleHistoryRepository(IApplicationDbContext context, ILogger<VehicleHistoryRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> AddAsync(VehicleHistoryCreateDTO entity)
        {
            try
            {
                var vehicleHistory = new Entity.Model.VehicleHistory // Asumiendo que VehicleHistory está en Entity.Model
                {
                    totaltime = entity.TotalTime,
                    id_registeredvehicle = entity.RegisteredVehicleId,
                    id_typevehicle = entity.TypeVehicleId
                };

                _context.VehicleHistories.Add(vehicleHistory); // Usa Add, no AddAsync
                await _context.SaveChangesAsync();
                return vehicleHistory.id;
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
                var vehicleHistoryToDelete = await _context.VehicleHistories.FindAsync(id);
                if (vehicleHistoryToDelete != null)
                {
                    _context.VehicleHistories.Remove(vehicleHistoryToDelete);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
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
                return await _context.VehicleHistories
                    .Select(vh => new VehicleHistoryDTO
                    {
                        id = vh.id,
                        totaltime = vh.totaltime,
                        id_registeredvehicle = vh.id_registeredvehicle,
                        id_typevehicle = vh.id_typevehicle
                    })
                    .ToListAsync();
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
                return await _context.VehicleHistories
                    .Where(vh => vh.id == id)
                    .Select(vh => new VehicleHistoryDTO
                    {
                        id = vh.id,
                        totaltime = vh.totaltime,
                        id_registeredvehicle = vh.id_registeredvehicle,
                        id_typevehicle = vh.id_typevehicle
                    })
                    .FirstOrDefaultAsync();
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
                var vehicleHistoryToUpdate = await _context.VehicleHistories.FindAsync(id);
                if (vehicleHistoryToUpdate != null)
                {
                    vehicleHistoryToUpdate.totaltime = entity.TotalTime;
                    vehicleHistoryToUpdate.id_registeredvehicle = entity.RegisteredVehicleId;
                    vehicleHistoryToUpdate.id_typevehicle = entity.TypeVehicleId;

                    _context.Entry(vehicleHistoryToUpdate).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el historial del vehículo con ID: {VehicleHistoryId}", id);
                return false;
            }
        }
    }
}

