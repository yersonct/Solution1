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
        protected readonly IApplicationDbContextWithEntry _context;
        private readonly ILogger<VehicleRepository> _logger;

        public VehicleRepository(IApplicationDbContextWithEntry context, ILogger<VehicleRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Vehicle> AddAsync(Vehicle entity)
        {
            try
            {
                entity.active = true; // Establecer como activo al crear
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
                    vehicleToDelete.active = false; // Eliminación lógica
                    _context.Entry(vehicleToDelete).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar (lógicamente) el vehículo con ID: {VehicleId}", id);
                return false;
            }
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            try
            {
                return await _context.Set<Vehicle>()
                    .Include(v => v.client)
                    .Where(v => v.active) // Filtrar solo los activos
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los vehículos activos.");
                return new List<Vehicle>();
            }
        }

        public async Task<Vehicle?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Vehicle>()
                    .Include(v => v.client)
                    .FirstOrDefaultAsync(v => v.id == id && v.active); // Filtrar solo los activos
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el vehículo activo con ID: {VehicleId}", id);
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
                _logger.LogError(ex, "Error de concurrencia al actualizar el vehículo activo con ID: {VehicleId}", entity.id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el vehículo activo con ID: {VehicleId}", entity.id);
                return false;
            }
        }
    }
}