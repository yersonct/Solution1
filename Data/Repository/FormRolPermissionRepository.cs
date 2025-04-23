using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Data.Interfaces;
using Entity.Context;
using Entity.DTOs;
using Entity.Model;
using Dapper;

namespace Data.Repository
{
    public class FormRolPermissionRepository : IFormRolPermissionRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly ILogger<FormRolPermissionRepository> _logger;

        public FormRolPermissionRepository(ApplicationDbContext context, ILogger<FormRolPermissionRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<FormRolPermissionDTO>> GetAllAsync()
        {
            try
            {
                return await _context.FormRolPermission
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
                return await _context.FormRolPermission
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

                _context.FormRolPermission.Add(entity);
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
                var entity = await _context.FormRolPermission.FindAsync(dto.id);
                if (entity == null) return false;

                entity.id_forms = _context.Forms.SingleOrDefault(f => f.name == dto.formName)?.id ?? 0;
                entity.id_rol = _context.Rol.SingleOrDefault(r => r.Name == dto.rolName)?.id ?? 0;
                entity.id_permission = _context.Permission.SingleOrDefault(p => p.name == dto.permissionName)?.id ?? 0;
                entity.active = dto.active;

                if (entity.id_forms == 0 || entity.id_rol == 0 || entity.id_permission == 0)
                {
                    _logger.LogError("Error updating FormRolPermission: Invalid Form, Rol, or Permission name.");
                    return false;
                }

                _context.FormRolPermission.Update(entity);
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
                var entity = await _context.FormRolPermission.FindAsync(id);
                if (entity == null) return false;

                entity.active = false; // Set active to false for logical delete
                _context.FormRolPermission.Update(entity);
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