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
    public class RegisteredVehicleRepository : IRegisteredVehicleRepository
    {
        private readonly IApplicationDbContextWithEntry _context; // Cambiado a IApplicationDbContext
        private readonly ILogger<RegisteredVehicleRepository> _logger;

        public RegisteredVehicleRepository(IApplicationDbContextWithEntry context, ILogger<RegisteredVehicleRepository> logger) // Cambiado a IApplicationDbContext
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> CanConnectAsync()
        {
            try
            {
                return await _context.Database.CanConnectAsync(); // Usar _context.Database
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar la conexión a la base de datos.");
                return false;
            }
        }

        public async Task<RegisteredVehicle> AddAsync(RegisteredVehicle entity)
        {
            try
            {
                await _context.Set<RegisteredVehicle>().AddAsync(entity); // Usar _context.Set<RegisteredVehicle>()
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar el vehículo registrado.");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var registeredVehicleToDelete = await _context.Set<RegisteredVehicle>().FindAsync(id); // Usar _context.Set<RegisteredVehicle>()
                if (registeredVehicleToDelete != null)
                {
                    _context.Set<RegisteredVehicle>().Remove(registeredVehicleToDelete); // Usar _context.Set<RegisteredVehicle>()
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el vehículo registrado con ID: {RegisteredVehicleId}", id);
                return false;
            }
        }

        public async Task<IEnumerable<RegisteredVehicle>> GetAllAsync()
        {
            try
            {
                return await _context.Set<RegisteredVehicle>() // Usar _context.Set<RegisteredVehicle>()
                    .Include(rv => rv.vehicle) // Make sure to include navigation properties if needed
                    .Include(rv => rv.vehiclehistory)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los vehículos registrados.");
                return new List<RegisteredVehicle>();
            }
        }

        public async Task<RegisteredVehicle?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<RegisteredVehicle>() // Usar _context.Set<RegisteredVehicle>()
                    .Include(rv => rv.vehicle) // Make sure to include navigation properties if needed
                    .Include(rv => rv.vehiclehistory)
                    .FirstOrDefaultAsync(rv => rv.id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el vehículo registrado con ID: {RegisteredVehicleId}", id);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(RegisteredVehicle entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Error de concurrencia al actualizar el vehículo registrado con ID: {RegisteredVehicleId}", entity.id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el vehículo registrado con ID: {RegisteredVehicleId}", entity.id);
                return false;
            }
        }
    }
}