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
                    .Select(x => new FormRolPermissionDTO
                    {
                        id = x.id,
                        id_forms = x.id_forms,
                        FormsName = x.Forms.name,
                        id_rol = x.id_rol,
                        RolName = x.Rol.Name,
                        id_permission = x.id_permission,
                        PermissionName = x.Permission.name
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los FormRolPermissions.");
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
                    .Where(x => x.id == id)
                    .Select(x => new FormRolPermissionDTO
                    {
                        id = x.id,
                        id_forms  = x.id_forms,
                        FormsName = x.Forms.name,
                        id_rol = x.id_rol,
                        RolName = x.Rol.Name,
                        id_permission = x.id_permission,
                        PermissionName = x.Permission.name
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener FormRolPermission con ID: {Id}", id);
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
                    id_permission = dto.id_permission
                };

                _context.FormRolPermission.Add(entity);
                await _context.SaveChangesAsync();

                return await GetByIdAsync(entity.id)
                       ?? throw new Exception("No se pudo recuperar el FormRolPermission creado.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear FormRolPermission.");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(FormRolPermissionDTO dto)
        {
            try
            {
                var entity = await _context.FormRolPermission.FindAsync(dto.id);
                if (entity == null) return false;

                entity.id_forms = dto.id_forms;
                entity.id_rol = dto.id_rol;
                entity.id_permission = dto.id_permission;

                _context.FormRolPermission.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar FormRolPermission con ID: {Id}.", dto.id);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.FormRolPermission.FindAsync(id);
                if (entity == null) return false;

                _context.FormRolPermission.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar FormRolPermission con ID: {Id}.", id);
                return false;
            }
        }
    }
}