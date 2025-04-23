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
                entity.active = true; // Set active to true on add
                await _context.Set<Permission>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding permission.");
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
                    permissionToDelete.active = false; // Set active to false for logical delete
                    _context.Entry(permissionToDelete).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logically deleting permission with ID: {PermissionId}", id);
                return false;
            }
        }

        public async Task<IEnumerable<Permission>> GetAllAsync()
        {
            try
            {
                return await _context.Set<Permission>()
                    .Include(p => p.FormRolPermissions)
                        .ThenInclude(frp => frp.Forms)
                    .Where(p => p.active) // Filter for active permissions
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all permissions.");
                return new List<Permission>();
            }
        }

        public async Task<Permission?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Permission>()
                    .Include(p => p.FormRolPermissions)
                        .ThenInclude(frp => frp.Forms)
                    .FirstOrDefaultAsync(u => u.id == id && u.active); // Get only active permission
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting permission with ID: {PermissionId}", id);
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
                _logger.LogError(ex, "Concurrency error updating permission with ID: {PermissionId}", entity.id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating permission with ID: {PermissionId}", entity.id);
                return false;
            }
        }
    }
}