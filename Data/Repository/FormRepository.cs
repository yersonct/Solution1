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
    public class FormRepository : IFormRepository
    {
        private readonly IApplicationDbContextWithEntry _context; // Cambiado a IApplicationDbContext
        private readonly ILogger<FormRepository> _logger;

        public FormRepository(IApplicationDbContextWithEntry context, ILogger<FormRepository> logger) // Cambiado a IApplicationDbContext
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Forms> AddAsync(Forms entity)
        {
            try
            {
                entity.Active = true; // Set active to true when adding a new form
                await _context.Set<Forms>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding form.");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var formToDelete = await _context.Set<Forms>().FindAsync(id);
                if (formToDelete != null)
                {
                    formToDelete.Active = false; // Set active to false instead of deleting
                    _context.Entry(formToDelete).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logically deleting form with ID: {FormId}", id);
                return false;
            }
        }

        public async Task<IEnumerable<Forms>> GetAllAsync()
        {
            try
            {
                return await _context.Set<Forms>()
                    .Include(u => u.FormModules)
                    .Where(f => f.Active) // Filter out inactive forms
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all forms.");
                return new List<Forms>();
            }
        }

        public async Task<Forms?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Forms>()
                    .Include(u => u.FormModules)
                    .FirstOrDefaultAsync(u => u.Id == id && u.Active); // Get only active forms
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting form with ID: {FormId}", id);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(Forms entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error updating form with ID: {FormId}", entity.Id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating form with ID: {FormId}", entity.Id);
                return false;
            }
        }
    }
}