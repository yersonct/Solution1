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
    public class ParkingRepository : IParkingRepository
    {
        private readonly IApplicationDbContext _context; // Cambiado a IApplicationDbContext
        private readonly ILogger<ParkingRepository> _logger;

        public ParkingRepository(IApplicationDbContext context, ILogger<ParkingRepository> logger) // Cambiado a IApplicationDbContext
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Parking> AddAsync(Parking entity)
        {
            try
            {
                await _context.Set<Parking>().AddAsync(entity); // Usar _context.Set<Parking>()
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar el parking.");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var parkingToDelete = await _context.Set<Parking>().FindAsync(id); // Usar _context.Set<Parking>()
                if (parkingToDelete != null)
                {
                    _context.Set<Parking>().Remove(parkingToDelete); // Usar _context.Set<Parking>()
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el parking con ID: {ParkingId}", id);
                return false;
            }
        }

        public async Task<IEnumerable<Parking>> GetAllAsync()
        {
            try
            {
                return await _context.Set<Parking>() // Usar _context.Set<Parking>()
                    .Include(p => p.camara) // Solo si tienes esta propiedad configurada
                    .Include(p => p.VehicleHistoryParkingRates)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los parkings.");
                return new List<Parking>();
            }
        }

        public async Task<Parking?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Parking>() // Usar _context.Set<Parking>()
                    .Include(p => p.camara) // Solo si tienes esta propiedad configurada
                    .Include(p => p.VehicleHistoryParkingRates)
                    .FirstOrDefaultAsync(p => p.id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el parking con ID: {ParkingId}", id);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(Parking entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Error de concurrencia al actualizar el parking con ID: {ParkingId}", entity.id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el parking con ID: {ParkingId}", entity.id);
                return false;
            }
        }
    }
}