using Data.Interfaces;
using Entity.Context;
using Entity.DTOs;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class FormRolPermissionRepository : IFormRolPermissionRepository
    {
        private readonly IApplicationDbContext _context; // Cambiado a IApplicationDbContext
        private readonly ILogger<FormRolPermissionRepository> _logger;

        public FormRolPermissionRepository(IApplicationDbContext context, ILogger<FormRolPermissionRepository> logger) // Cambiado a IApplicationDbContext
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<FormRolPermissionDTO>> GetAllAsync()
        {
            try
            {
                return await _context.Set<FormRolPermission>()
                    .Include(x => x.Forms)
                    .Include(x => x.Rol)
                    .Include(x => x.Permission)
                    .Where(x => x.active) // Filter for active FormRolPermissions
                    .Select(x => new FormRolPermissionDTO
                    {
                        id = x.id,
                        formName = x.Forms.name,
                        rolName = x.Rol.Name,
                        permissionName = x.Permission.name,
                        active = x.active
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all FormRolPermissions.");
                return new List<FormRolPermissionDTO>();
            }
        }

        public async Task<FormRolPermissionDTO?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<FormRolPermission>()
                    .Include(x => x.Forms)
                    .Include(x => x.Rol)
                    .Include(x => x.Permission)
                    .Where(x => x.id == id && x.active) // Filter for active FormRolPermission
                    .Select(x => new FormRolPermissionDTO
                    {
                        id = x.id,
                        formName = x.Forms.name,
                        rolName = x.Rol.Name,
                        permissionName = x.Permission.name,
                        active = x.active
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting FormRolPermission with ID: {Id}", id);
                return null;
            }
        }

        public async Task<FormRolPermissionDTO> AddAsync(FormRolPermissionCreateDTO dto)
        {
            try
            {
                var entity = new FormRolPermission
                {
                    id_forms = dto.id_forms,
                    id_rol = dto.id_rol,
                    id_permission = dto.id_permission,
                    active = true // Set active to true on creation
                };

                _context.Set<FormRolPermission>().Add(entity);
                await _context.SaveChangesAsync();

                return await GetByIdAsync(entity.id)
                       ?? throw new Exception("Could not retrieve the created FormRolPermission.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating FormRolPermission.");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(FormRolPermissionDTO dto)
        {
            try
            {
                var entity = await _context.Set<FormRolPermission>().FindAsync(dto.id);
                if (entity == null) return false;

                // Assuming you have DbSet properties for Forms, Rol, and Permission in IApplicationDbContext
                entity.id_forms = await _context.Set<Forms>().Where(f => f.name == dto.formName).Select(f => f.id).SingleOrDefaultAsync();
                entity.id_rol = await _context.Set<Rol>().Where(r => r.Name == dto.rolName).Select(r => r.id).SingleOrDefaultAsync();
                entity.id_permission = await _context.Set<Permission>().Where(p => p.name == dto.permissionName).Select(p => p.id).SingleOrDefaultAsync();
                entity.active = dto.active;

                if (entity.id_forms == 0 || entity.id_rol == 0 || entity.id_permission == 0)
                {
                    _logger.LogError("Error updating FormRolPermission: Invalid Form, Rol, or Permission name.");
                    return false;
                }

                _context.Set<FormRolPermission>().Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating FormRolPermission with ID: {Id}.", dto.id);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.Set<FormRolPermission>().FindAsync(id);
                if (entity == null) return false;

                entity.active = false; // Set active to false for logical delete
                _context.Set<FormRolPermission>().Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logically deleting FormRolPermission with ID: {Id}.", id);
                return false;
            }
        }
    }
}