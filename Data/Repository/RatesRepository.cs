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
    public class RatesRepository : IRatesRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly ILogger<RatesRepository> _logger;

        public RatesRepository(ApplicationDbContext context, ILogger<RatesRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Rates> AddAsync(Rates entity)
        {
            try
            {
                await _context.Set<Rates>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar la tarifa.");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var ratesToDelete = await _context.Set<Rates>().FindAsync(id);
                if (ratesToDelete != null)
                {
                    _context.Set<Rates>().Remove(ratesToDelete);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la tarifa con ID: {RatesId}", id);
                return false;
            }
        }

        public async Task<IEnumerable<Rates>> GetAllAsync()
        {
            try
            {
                return await _context.Set<Rates>()
                    .Include(r => r.TypeRates) // Assuming you have a navigation property to TypeRates
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las tarifas.");
                return new List<Rates>();
            }
        }

        public async Task<Rates?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Rates>()
                    .Include(r => r.TypeRates) // Assuming you have a navigation property to TypeRates
                    .FirstOrDefaultAsync(r => r.id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la tarifa con ID: {RatesId}", id);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(Rates entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Error de concurrencia al actualizar la tarifa con ID: {RatesId}", entity.id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la tarifa con ID: {RatesId}", entity.id);
                return false;
            }
        }
    }
}
