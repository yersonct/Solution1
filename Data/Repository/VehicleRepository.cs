using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Data.Interfaces;
using Entity.Context;
using Entity.Model;
using Dapper;

namespace Data.Repository
{
    public class VehicleRepository : IVehicleRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly ILogger<VehicleRepository> _logger;

        public VehicleRepository(ApplicationDbContext context, ILogger<VehicleRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Vehicle> AddAsync(Vehicle entity)
        {
            try
            {
                await _context.Set<Vehicle>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar el vehículo.");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var vehicleToDelete = await _context.Set<Vehicle>().FindAsync(id);
                if (vehicleToDelete != null)
                {
                    _context.Set<Vehicle>().Remove(vehicleToDelete);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el vehículo con ID: {VehicleId}", id);
                return false;
            }
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            try
            {
                return await _context.Set<Vehicle>()
                    .Include(v => v.client)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los vehículos.");
                return new List<Vehicle>();
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
                _logger.LogError(ex, "Error al obtener el vehículo con ID: {VehicleId}", id);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(Vehicle entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Error de concurrencia al actualizar el vehículo con ID: {VehicleId}", entity.id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el vehículo con ID: {VehicleId}", entity.id);
                return false;
            }
        }
    }
}