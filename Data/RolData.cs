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
    public class RolData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RolData> _logger; // Corregido: ILogger<RolData>
        public RolData(ApplicationDbContext context, ILogger<RolData> logger) // Corregido: ILogger<UserData>
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Rol>> GetAllAsyncSQL()
        {
            string query = @"SELECT Id, Name,Description
	                        FROM role;";

            return await _context.QueryAsync<Rol>(query);
        }

        public async Task<IEnumerable<Rol>> GetAllAsync()
        {
            return await _context.Set<Rol>().Include(u => u.FormRolPermissions).Include(u => u.RolUsers).ToListAsync();
        }

        public async Task<Rol?> GetByIdAsyncSQL(int id)
        {
            try
            {
                string query = @"SELECT Id, Name,Description
	            FROM role WHERE Id =@Id ";

                return await _context.QueryFirstOrDefaultAsync<Rol>(query, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener un usuario con ID {RolId}", id);
                throw;
            }
        }

        public async Task<Rol?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Rol>().Include(u => u.FormRolPermissions).Include(u => u.RolUsers).FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener un usuario con ID {RolId}", id);
                throw;
            }
        }

        public async Task<Rol> CreateAsyncSQL(Rol Rol)
        {
            try
            {
                string query = @"
                                INSERT INTO role (Name,Description) 
                                VALUES (@Name,@Description)
                                RETURNING Id;";


                Rol.Id = await _context.QuerySingleAsync<int>(query, new
                {
                    Rol.Name,
                    Rol.Description

                });

                //Rol.Id = newId;
                return Rol;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<Rol> CreateAsync(Rol Rol)
        {
            try
            {
                await _context.Set<Rol>().AddAsync(Rol);
                await _context.SaveChangesAsync();
                return Rol;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsyncSQL(Rol Rol)
        {
            try
            {
                string query = @"
                                UPDATE role
                                SET Name = @Name,Description=@Description
                                WHERE Id = @Id;
                                SELECT 1;";

                int rowsAffected = await _context.QuerySingleAsync<int>(query, new
                {
                    Rol.Id,
                    Rol.Name,
                    Rol.Description

                });

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el usuario: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Rol Rol)
        {
            try
            {
                _context.Set<Rol>().Update(Rol);
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
                  DELETE FROM role WHERE id = @Id RETURNING 1;";

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
                var Rol = await GetByIdAsync(id);
                if (Rol == null)
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
