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
    public class RolRepository : IRolRepository
    {
        private readonly IApplicationDbContext _context; // Cambiado a IApplicationDbContext
        private readonly ILogger<RolRepository> _logger;

        public RolRepository(IApplicationDbContext context, ILogger<RolRepository> logger) // Cambiado a IApplicationDbContext
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Rol> AddAsync(Rol entity)
        {
            try
            {
                entity.Active = true; // Establecer Active en true al agregar
                await _context.Set<Rol>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar el rol.");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var rolToDelete = await _context.Set<Rol>().FindAsync(id);
                if (rolToDelete != null)
                {
                    rolToDelete.Active = false; // Marcar como inactivo en lugar de eliminar
                    _context.Entry(rolToDelete).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar (lógicamente) el rol con ID: {RolId}", id);
                return false;
            }
        }

        public async Task<IEnumerable<Rol>> GetAllAsync()
        {
            try
            {
                return await _context.Set<Rol>()
                    .Include(r => r.FormRolPermissions)
                        .ThenInclude(frp => frp.Forms)
                    .Include(r => r.RolUsers)
                    .Where(r => r.Active) // Filtrar solo roles activos
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los roles.");
                return new List<Rol>();
            }
        }

        public async Task<Rol?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Rol>()
                    .Include(r => r.FormRolPermissions)
                        .ThenInclude(frp => frp.Forms)
                    .Include(r => r.RolUsers)
                    .FirstOrDefaultAsync(r => r.id == id && r.Active); // Filtrar solo roles activos
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el rol con ID: {RolId}", id);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(Rol entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Error de concurrencia al actualizar el rol con ID: {RolId}", entity.id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el rol con ID: {RolId}", entity.id);
                return false;
            }
        }
    }
}