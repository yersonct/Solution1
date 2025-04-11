using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Entity.Context;
using Entity.DTOs;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class FormRolPermissionData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FormRolPermissionData> _logger;

        public FormRolPermissionData(ApplicationDbContext context, ILogger<FormRolPermissionData> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // LINQ - Obtener todos
        public async Task<IEnumerable<FormRolPermissionDTO>> GetAllAsync()
        {
            return await _context.FormRolPermissions
                .Include(x => x.Forms)
                .Include(x => x.Rol)
                .Include(x => x.Permission)
                .Select(x => new FormRolPermissionDTO
                {
                    Id = x.Id,
                    FormId = x.id_forms,
                    FormName = x.Forms.Name,
                    RolId = x.id_role,
                    RolName = x.Rol.Name,
                    PermissionId = x.id_permission,
                    PermissionName = x.Permission.Name
                })
                .ToListAsync();
        }

        // LINQ - Obtener por ID
        public async Task<FormRolPermissionDTO?> GetByIdAsync(int id)
        {
            return await _context.FormRolPermissions
                .Include(x => x.Forms)
                .Include(x => x.Rol)
                .Include(x => x.Permission)
                .Where(x => x.Id == id)
                .Select(x => new FormRolPermissionDTO
                {
                    Id = x.Id,
                    FormId = x.id_forms,
                    FormName = x.Forms.Name,
                    RolId = x.id_role,
                    RolName = x.Rol.Name,
                    PermissionId = x.id_permission,
                    PermissionName = x.Permission.Name
                })
                .FirstOrDefaultAsync();
        }

        // LINQ - Crear
        public async Task<FormRolPermissionDTO> CreateAsync(FormRolPermissionCreateDTO dto)
        {
            try
            {
                var entity = new FormRolPermission
                {
                    id_forms = dto.id_forms,
                    id_role = dto.id_role,
                    id_permission = dto.id_permission
                };

                _context.FormRolPermissions.Add(entity);
                await _context.SaveChangesAsync();

                return await GetByIdAsync(entity.Id)
                    ?? throw new Exception();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear FormRolPermission");
                throw;
            }
        }

        // LINQ - Actualizar
        public async Task<bool> UpdateAsync(FormRolPermissionDTO dto)
        {
            try
            {
                var entity = await _context.FormRolPermissions.FindAsync(dto.Id);
                if (entity == null) return false;

                entity.id_forms = dto.FormId;
                entity.id_role = dto.RolId;
                entity.id_permission = dto.PermissionId;

                _context.FormRolPermissions.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar FormRolPermission con ID {Id} (LINQ)", dto.Id);
                return false;
            }
        }

        // LINQ - Eliminar
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.FormRolPermissions.FindAsync(id);
                if (entity == null) return false;

                _context.FormRolPermissions.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar FormRolPermission con ID {Id}", id);
                return false;
            }
        }

        // SQL - Obtener todos
        public async Task<IEnumerable<FormRolPermissionDTO>> GetAllAsyncSQL()
        {
            var sql = @"
                SELECT frp.id,
                       frp.id_forms AS FormId, f.name AS FormName,
                       frp.id_role AS RolId, r.name AS RolName,
                       frp.id_permission AS PermissionId, p.name AS PermissionName
                FROM formrolpermission frp
                JOIN forms f ON frp.id_forms = f.id
                JOIN role r ON frp.id_role = r.id
                JOIN permission p ON frp.id_permission = p.id";

            return await _context.QueryAsync<FormRolPermissionDTO>(sql);
        }

        // SQL - Obtener por ID
        public async Task<FormRolPermissionDTO?> GetByIdAsyncSQL(int id)
        {
            var sql = @"
                SELECT frp.id,
                       frp.id_forms AS FormId, f.name AS FormName,
                       frp.id_role AS RolId, r.name AS RolName,
                       frp.id_permission AS PermissionId, p.name AS PermissionName
                FROM formrolpermission frp
                JOIN forms f ON frp.id_forms = f.id
                JOIN role r ON frp.id_role = r.id
                JOIN permission p ON frp.id_permission = p.id
                WHERE frp.id = @Id";

            return await _context.QueryFirstOrDefaultAsync<FormRolPermissionDTO>(sql, new { Id = id });
        }

        // SQL - Crear
        public async Task<FormRolPermissionDTO> CreateAsyncSQL(FormRolPermissionCreateDTO dto)
        {
            try
            {
                var sql = @"
            INSERT INTO formrolpermission (id_forms, id_role, id_permission)
            VALUES (@id_forms, @id_role, @id_permission)
            RETURNING id;";

                var newId = await _context.QuerySingleAsync<int>(sql, new
                {
                    dto.id_forms,
                    dto.id_role,
                    dto.id_permission
                });

                return await GetByIdAsyncSQL(newId)
                    ?? throw new Exception("No se pudo recuperar el FormRolPermission creado.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear FormRolPermission (SQL)");
                throw;
            }
        }


        // SQL - Actualizar
        public async Task<bool> UpdateAsyncSQL(FormRolPermissionDTO dto)
        {
            try
            {
                var query = @"
                    UPDATE formrolpermission 
                    SET id_forms = @FormId,
                        id_role = @RolId,
                        id_permission = @PermissionId
                    WHERE id = @Id;
                    SELECT 1;";

                int rowsAffected = await _context.QuerySingleAsync<int>(query, new
                {
                    dto.Id,
                    dto.FormId,
                    dto.RolId,
                    dto.PermissionId
                });

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar FormRolPermission con ID {Id} (SQL)", dto.Id);
                return false;
            }
        }

        // SQL - Eliminar
        public async Task<bool> DeleteAsyncSQL(int id)
        {
            try
            {
                var sql = @"DELETE FROM formrolpermission WHERE id = @Id RETURNING 1;";
                int result = await _context.QuerySingleAsync<int>(sql, new { Id = id });
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar FormRolPermission con ID {Id} (SQL)", id);
                return false;
            }
        }

        public async Task<bool> ExistsByIdAsync(string v, object rolid)
        {
            throw new NotImplementedException();
        }
    }
}
