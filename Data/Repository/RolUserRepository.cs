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
    public class RolUserRepository : IRolUserRepository
    {
        private readonly IApplicationDbContextWithEntry _context; // Cambiado a IApplicationDbContext
        private readonly ILogger<RolUserRepository> _logger;

        public RolUserRepository(IApplicationDbContextWithEntry context, ILogger<RolUserRepository> logger) // Cambiado a IApplicationDbContext
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<RolUser> AddAsync(RolUser entity)
        {
            try
            {
                await _context.Set<RolUser>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar la relación Rol-Usuario.");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var rolUserToDelete = await _context.Set<RolUser>().FindAsync(id);
                if (rolUserToDelete != null)
                {
                    _context.Set<RolUser>().Remove(rolUserToDelete);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la relación Rol-Usuario con ID: {Id}", id);
                return false;
            }
        }

        public async Task<IEnumerable<RolUser>> GetAllAsync()
        {
            try
            {
                return await _context.Set<RolUser>()
                    .Include(r => r.Rol)
                    .Include(r => r.User)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las relaciones Rol-Usuario.");
                return new List<RolUser>();
            }
        }

        public async Task<RolUser?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<RolUser>()
                    .Include(r => r.Rol)
                    .Include(r => r.User)
                    .FirstOrDefaultAsync(r => r.id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la relación Rol-Usuario con ID: {Id}", id);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(RolUser entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Error de concurrencia al actualizar la relación Rol-Usuario con ID: {Id}", entity.id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la relación Rol-Usuario con ID: {Id}", entity.id);
                return false;
            }
        }
    }
}