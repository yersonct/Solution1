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
    public class TypeRatesRepository : ITypeRatesRepository
    {
        private readonly IApplicationDbContext _context; // Cambiado a IApplicationDbContext
        private readonly ILogger<TypeRatesRepository> _logger;

        public TypeRatesRepository(IApplicationDbContext context, ILogger<TypeRatesRepository> logger) // Cambiado a IApplicationDbContext
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TypeRates> AddAsync(TypeRates entity)
        {
            try
            {
                await _context.Set<TypeRates>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar el tipo de tarifa.");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var typeRatesToDelete = await _context.Set<TypeRates>().FindAsync(id);
                if (typeRatesToDelete != null)
                {
                    _context.Set<TypeRates>().Remove(typeRatesToDelete);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el tipo de tarifa con ID: {TypeRatesId}", id);
                return false;
            }
        }

        public async Task<IEnumerable<TypeRates>> GetAllAsync()
        {
            try
            {
                return await _context.Set<TypeRates>()
                    .Include(tr => tr.rates)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los tipos de tarifa.");
                return new List<TypeRates>();
            }
        }

        public async Task<TypeRates?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<TypeRates>()
                    .Include(tr => tr.rates)
                    .FirstOrDefaultAsync(tr => tr.id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el tipo de tarifa con ID: {TypeRatesId}", id);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(TypeRates entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Error de concurrencia al actualizar el tipo de tarifa con ID: {TypeRatesId}", entity.id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el tipo de tarifa con ID: {TypeRatesId}", entity.id);
                return false;
            }
        }
    }
}