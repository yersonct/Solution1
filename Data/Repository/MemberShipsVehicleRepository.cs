using Data.Interfaces;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class MembershipsVehicleRepository : IMembershipsVehicleRepository
    {
        private readonly IApplicationDbContextWithEntry _context; // Cambiado a IApplicationDbContext
        private readonly ILogger<MembershipsVehicleRepository> _logger;

        public MembershipsVehicleRepository(IApplicationDbContextWithEntry context, ILogger<MembershipsVehicleRepository> logger) // Cambiado a IApplicationDbContext
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<MembershipsVehicle> AddAsync(MembershipsVehicle entity)
        {
            try
            {
                entity.active = true; // Establecer como activo al crear
                await _context.Set<MembershipsVehicle>().AddAsync(entity); // Usar _context.Set<MembershipsVehicle>()
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar la relación MembershipsVehicle.");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var membershipsVehicleToDelete = await _context.Set<MembershipsVehicle>().FindAsync(id); // Usar _context.Set<MembershipsVehicle>()
                if (membershipsVehicleToDelete != null)
                {
                    membershipsVehicleToDelete.active = false; // Eliminación lógica
                    _context.Entry(membershipsVehicleToDelete).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar (lógicamente) la relación MembershipsVehicle con ID: {MembershipsVehicleId}", id);
                return false;
            }
        }

        public async Task<IEnumerable<MembershipsVehicle>> GetAllAsync()
        {
            try
            {
                return await _context.Set<MembershipsVehicle>() // Usar _context.Set<MembershipsVehicle>()
                    .Include(mv => mv.vehicle)
                    .Include(mv => mv.memberships)
                    .Where(mv => mv.active) // Filtrar solo los activos
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las relaciones MembershipsVehicle activas.");
                return new List<MembershipsVehicle>();
            }
        }

        public async Task<MembershipsVehicle?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<MembershipsVehicle>() // Usar _context.Set<MembershipsVehicle>()
                    .Include(mv => mv.vehicle)
                    .Include(mv => mv.memberships)
                    .FirstOrDefaultAsync(mv => mv.id == id && mv.active); // Filtrar solo los activos
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la relación MembershipsVehicle activa con ID: {MembershipsVehicleId}", id);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(MembershipsVehicle entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Error de concurrencia al actualizar la relación MembershipsVehicle activa con ID: {MembershipsVehicleId}", entity.id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la relación MembershipsVehicle activa con ID: {MembershipsVehicleId}", entity.id);
                return false;
            }
        }
    }
}