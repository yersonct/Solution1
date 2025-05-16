using Data.Interfaces;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class VehicleHistoryParkingRatesRepository : IVehicleHistoryParkingRatesRepository
    {
        private readonly IApplicationDbContext _context; // Cambiado a IApplicationDbContext
        private readonly ILogger<VehicleHistoryParkingRatesRepository> _logger;

        public VehicleHistoryParkingRatesRepository(IApplicationDbContext context, ILogger<VehicleHistoryParkingRatesRepository> logger) // Cambiado a IApplicationDbContext
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<VehicleHistoryParkingRates> AddAsync(VehicleHistoryParkingRates entity)
        {
            try
            {
                await _context.Set<VehicleHistoryParkingRates>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar VehicleHistoryParkingRates.");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var toDelete = await _context.Set<VehicleHistoryParkingRates>().FindAsync(id);
                if (toDelete != null)
                {
                    _context.Set<VehicleHistoryParkingRates>().Remove(toDelete);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar VehicleHistoryParkingRates con ID: {Id}", id);
                return false;
            }
        }

        public async Task<IEnumerable<VehicleHistoryParkingRates>> GetAllAsync()
        {
            try
            {
                return await _context.Set<VehicleHistoryParkingRates>().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los VehicleHistoryParkingRates.");
                return new List<VehicleHistoryParkingRates>();
            }
        }

        public async Task<VehicleHistoryParkingRates?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<VehicleHistoryParkingRates>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener VehicleHistoryParkingRates con ID: {Id}", id);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(VehicleHistoryParkingRates entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Error de concurrencia al actualizar VehicleHistoryParkingRates con ID: {Id}", entity.id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar VehicleHistoryParkingRates con ID: {Id}", entity.id);
                return false;
            }
        }
    }
}