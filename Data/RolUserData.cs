using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class RolUserData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RolUserData> _logger;

        public RolUserData(ApplicationDbContext context, ILogger<RolUserData> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<RolUser>> GetAllAsyncSQL()
        {
            string query = @"SELECT id, id_role, id_user FROM roluser;";
            return await _context.QueryAsync<RolUser>(query);
        }

        public async Task<IEnumerable<RolUser>> GetAllAsync()
        {
            return await _context.Set<RolUser>()
                .Include(r => r.Rol)
                .Include(r => r.User)
                .ToListAsync();
        }

        public async Task<RolUser?> GetByIdAsyncSQL(int id)
        {
            try
            {
                string query = @"SELECT id, id_role, id_user FROM roluser WHERE id = @Id;";
                return await _context.QueryFirstOrDefaultAsync<RolUser>(query, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener RolUser con ID {Id}", id);
                throw;
            }
        }

        public async Task<RolUser?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<RolUser>()
                    .Include(r => r.Rol)
                    .Include(r => r.User)
                    .FirstOrDefaultAsync(r => r.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener RolUser con ID {Id}", id);
                throw;
            }
        }

        public async Task<RolUser> CreateAsyncSQL(RolUser rolUser)
        {
            try
            {
                string query = @"
                    INSERT INTO roluser (id_role, id_user)
                    VALUES (@rolid, @userid)
                    RETURNING id;";

                rolUser.Id = await _context.QuerySingleAsync<int>(query, new
                {
                    rolUser.RolId,
                    rolUser.UserId
                });

                return rolUser;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear RolUser: {ex.Message}");
                throw;
            }
        }

        public async Task<RolUser> CreateAsync(RolUser rolUser)
        {
            try
            {
                await _context.Set<RolUser>().AddAsync(rolUser);
                await _context.SaveChangesAsync();
                return rolUser;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear RolUser: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsyncSQL(RolUser rolUser)
        {
            try
            {
                string query = @"
                    UPDATE roluser
                    SET id_role = @rolid, id_user = @userid
                    WHERE id = @id;
                    SELECT 1;";

                int rowsAffected = await _context.QuerySingleAsync<int>(query, new
                {
                    rolUser.Id,
                    rolUser.RolId,
                    rolUser.UserId
                });

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar RolUser: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(RolUser rolUser)
        {
            try
            {
                _context.Set<RolUser>().Update(rolUser);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar RolUser: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsyncSQL(int id)
        {
            try
            {
                string query = @"DELETE FROM roluser WHERE id = @Id RETURNING 1;";
                int rowsAffected = await _context.QuerySingleAsync<int>(query, new { Id = id });
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar RolUser: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var rolUser = await GetByIdAsync(id);
                if (rolUser == null)
                    return false;

                _context.RolUsers.Remove(rolUser);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar RolUser: {ex.Message}");
                return false;
            }
        }
    }
}
