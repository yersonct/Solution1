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
    public class PermissionRepository : IPermissionRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly ILogger<PermissionRepository> _logger;

        public PermissionRepository(ApplicationDbContext context, ILogger<PermissionRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Permission> AddAsync(Permission entity)
        {
            try
            {
                await _context.Set<Permission>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar el permiso.");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var permissionToDelete = await _context.Set<Permission>().FindAsync(id);
                if (permissionToDelete != null)
                {
                    _context.Set<Permission>().Remove(permissionToDelete);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el permiso con ID: {PermissionId}", id);
                return false;
            }
        }

        public async Task<IEnumerable<Permission>> GetAllAsync()
        {
            try
            {
                return await _context.Set<Permission>()
                    .Include(p => p.FormRolPermissions)
                        .ThenInclude(frp => frp.Forms) // ¡Cargamos la entidad Forms!
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los permisos.");
                return new List<Permission>();
            }
        }

        public async Task<Permission?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Permission>()
                    .Include(p => p.FormRolPermissions)
                        .ThenInclude(frp => frp.Forms) // ¡Cargamos la entidad Forms!
                    .FirstOrDefaultAsync(u => u.id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el permiso con ID: {PermissionId}", id);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(Permission entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Error de concurrencia al actualizar el permiso con ID: {PermissionId}", entity.id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el permiso con ID: {PermissionId}", entity.id);
                return false;
            }
        }
    }
}