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
    public class TypeVehicleRepository : ITypeVehicleRepository
    {
        private readonly IApplicationDbContextWithEntry _context; // Cambiado a IApplicationDbContext
        private readonly ILogger<TypeVehicleRepository> _logger;

        public TypeVehicleRepository(IApplicationDbContextWithEntry context, ILogger<TypeVehicleRepository> logger) // Cambiado a IApplicationDbContext
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TypeVehicle> AddAsync(TypeVehicle entity)
        {
            try
            {
                await _context.Set<TypeVehicle>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar el tipo de vehículo.");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var typeVehicleToDelete = await _context.Set<TypeVehicle>().FindAsync(id);
                if (typeVehicleToDelete != null)
                {
                    _context.Set<TypeVehicle>().Remove(typeVehicleToDelete);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el tipo de vehículo con ID: {TypeVehicleId}", id);
                return false;
            }
        }

        public async Task<IEnumerable<TypeVehicle>> GetAllAsync()
        {
            try
            {
                return await _context.Set<TypeVehicle>().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los tipos de vehículo.");
                return new List<TypeVehicle>();
            }
        }

        public async Task<TypeVehicle?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<TypeVehicle>().FirstOrDefaultAsync(tv => tv.id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el tipo de vehículo con ID: {TypeVehicleId}", id);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(TypeVehicle entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Error de concurrencia al actualizar el tipo de vehículo con ID: {TypeVehicleId}", entity.id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el tipo de vehículo con ID: {TypeVehicleId}", entity.id);
                return false;
            }
        }
    }
}