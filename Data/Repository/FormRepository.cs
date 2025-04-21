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
        protected readonly ApplicationDbContext _context;
        private readonly ILogger<FormRepository> _logger;

        public FormRepository(ApplicationDbContext context, ILogger<FormRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Forms> AddAsync(Forms entity)
        {
            try
            {
                await _context.Set<Forms>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar el formulario.");
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
                    _context.Set<Forms>().Remove(formToDelete);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el formulario con ID: {FormId}", id);
                return false;
            }
        }

        public async Task<IEnumerable<Forms>> GetAllAsync()
        {
            try
            {
                return await _context.Set<Forms>().Include(u => u.FormModules).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los formularios.");
                return new List<Forms>();
            }
        }

        public async Task<Forms?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Forms>().Include(u => u.FormModules).FirstOrDefaultAsync(u => u.id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el formulario con ID: {FormId}", id);
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
                _logger.LogError(ex, "Error de concurrencia al actualizar el formulario con ID: {FormId}", entity.id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el formulario con ID: {FormId}", entity.id);
                return false;
            }
        }
    }
}
