using System;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class PermissionData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PermissionData> _logger; // Corregido: ILogger<PermissionData>
        public PermissionData(ApplicationDbContext context, ILogger<PermissionData> logger) // Corregido: ILogger<UserData>
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Permission>> GetAllAsyncSQL()
        {
            string query = @"SELECT Id, Name
	                        FROM Permission;";

            return await _context.QueryAsync<Permission>(query);
        }

        public async Task<IEnumerable<Permission>> GetAllAsync()
        {
            return await _context.Set<Permission>().Include(u => u.FormRolPermissions).ToListAsync();
        }

        public async Task<Permission?> GetByIdAsyncSQL(int id)
        {
            try
            {
                string query = @"SELECT Id, Name
	            FROM Permission WHERE Id =@Id ";

                return await _context.QueryFirstOrDefaultAsync<Permission>(query, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener un usuario con ID {PermissionId}", id);
                throw;
            }
        }

        public async Task<Permission?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Permission>().Include(u => u.FormRolPermissions).FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener un usuario con ID {PermissionId}", id);
                throw;
            }
        }

        public async Task<Permission> CreateAsyncSQL(Permission Permission)
        {
            try
            {
                string query = @"
                                INSERT INTO Permission (Name) 
                                VALUES (@Name)
                                RETURNING Id;";


                Permission.Id = await _context.QuerySingleAsync<int>(query, new
                {
                    Permission.Name,
       
                });

                //Permission.Id = newId;
                return Permission;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<Permission> CreateAsync(Permission Permission)
        {
            try
            {
                await _context.Set<Permission>().AddAsync(Permission);
                await _context.SaveChangesAsync();
                return Permission;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsyncSQL(Permission Permission)
        {
            try
            {
                string query = @"
                                UPDATE Permission 
                                SET Name = @Name
                                WHERE Id = @Id;
                                SELECT 1;";

                int rowsAffected = await _context.QuerySingleAsync<int>(query, new
                {
                    Permission.Id,
                    Permission.Name,


                });

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el usuario: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Permission Permission)
        {
            try
            {
                _context.Set<Permission>().Update(Permission);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el usuario: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsyncSQL(int id)
        {
            try
            {
                string query = @"
                  DELETE FROM Permission WHERE id = @Id RETURNING 1;";

                int rowsAffected = await _context.QuerySingleAsync<int>(query, new { Id = id });
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el usuario: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var Permission = await GetByIdAsync(id);
                if (Permission == null)
                    return false;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el usuario: {ex.Message}");
                return false;
            }
        }
    }
}
