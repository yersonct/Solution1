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
    public class ModuleRepository : IModuleRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly ILogger<ModuleRepository> _logger;

        public ModuleRepository(ApplicationDbContext context, ILogger<ModuleRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Modules> AddAsync(Modules entity)
        {
            try
            {
                await _context.Set<Modules>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar el módulo.");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var moduleToDelete = await _context.Set<Modules>().FindAsync(id);
                if (moduleToDelete != null)
                {
                    _context.Set<Modules>().Remove(moduleToDelete);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el módulo con ID: {ModuleId}", id);
                return false;
            }
        }

        public async Task<IEnumerable<Modules>> GetAllAsync()
        {
            try
            {
                return await _context.Set<Modules>().Include(u => u.FormModules).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los módulos.");
                return new List<Modules>();
            }
        }

        public async Task<Modules?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Modules>().Include(u => u.FormModules).FirstOrDefaultAsync(u => u.id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el módulo con ID: {ModuleId}", id);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(Modules entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Error de concurrencia al actualizar el módulo con ID: {ModuleId}", entity.id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el módulo con ID: {ModuleId}", entity.id);
                return false;
            }
        }
    }
}
