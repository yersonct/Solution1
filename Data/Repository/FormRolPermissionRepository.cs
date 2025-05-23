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
        private readonly IApplicationDbContextWithEntry _context; // Cambiado a IApplicationDbContext
        private readonly ILogger<FormRolPermissionRepository> _logger;

        public FormRolPermissionRepository(IApplicationDbContextWithEntry context, ILogger<FormRolPermissionRepository> logger) // Cambiado a IApplicationDbContext
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
                    .Where(x => x.Active) // Filter for active FormRolPermissions
                    .Select(x => new FormRolPermissionDTO
                    {
                        Id = x.Id,
                        FormName = x.Forms.Name,
                        RolName = x.Rol.Name,
                        PermissionName = x.Permission.Name,
                        Active = x.Active
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
                    .Where(x => x.Id == id && x.Active) // Filter for active FormRolPermission
                    .Select(x => new FormRolPermissionDTO
                    {
                        Id = x.Id,
                        FormName = x.Forms.Name,
                        RolName = x.Rol.Name,
                        PermissionName = x.Permission.Name,
                        Active = x.Active
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
                    FormId = dto.FormId,
                    RolId = dto.RolId,
                    PermissionId = dto.PermissionId,
                    Active = true // Set active to true on creation
                };

                _context.Set<FormRolPermission>().Add(entity);
                await _context.SaveChangesAsync();

                return await GetByIdAsync(entity.Id)
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
                var entity = await _context.Set<FormRolPermission>().FindAsync(dto.Id);
                if (entity == null) return false;

                // Assuming you have DbSet properties for Forms, Rol, and Permission in IApplicationDbContext
                entity.FormId = await _context.Set<Forms>().Where(f => f.Name == dto.FormName).Select(f => f.Id).SingleOrDefaultAsync();
                entity.RolId = await _context.Set<Rol>().Where(r => r.Name == dto.RolName).Select(r => r.Id).SingleOrDefaultAsync();
                entity.PermissionId = await _context.Set<Permission>().Where(p => p.Name == dto.PermissionName).Select(p => p.Id).SingleOrDefaultAsync();
                entity.Active = dto.Active;

                if (entity.FormId == 0 || entity.RolId == 0 || entity.PermissionId == 0)
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
                _logger.LogError(ex, "Error updating FormRolPermission with ID: {Id}.", dto.Id);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.Set<FormRolPermission>().FindAsync(id);
                if (entity == null) return false;

                entity.Active = false; // Set active to false for logical delete
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