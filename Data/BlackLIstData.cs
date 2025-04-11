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
    public class BlackListData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BlackListData> _logger;

        public BlackListData(ApplicationDbContext context, ILogger<BlackListData> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<BlackList>> GetAllAsyncSQL()
        {
            string query = @"SELECT id, reason, restrictiondate, id_client FROM blacklist;";
            return await _context.QueryAsync<BlackList>(query);
        }

        public async Task<IEnumerable<BlackList>> GetAllAsync()
        {
            return await _context.Set<BlackList>().Include(b => b.Client).ToListAsync();
        }

        public async Task<BlackList?> GetByIdAsyncSQL(int id)
        {
            try
            {
                string query = @"SELECT id, reason, restrictiondate, id_client FROM blacklist WHERE id = @Id;";
                return await _context.QueryFirstOrDefaultAsync<BlackList>(query, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el registro de la lista negra con ID {BlackListId}", id);
                throw;
            }
        }

        public async Task<BlackList?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<BlackList>().Include(b => b.Client).FirstOrDefaultAsync(b => b.id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el registro de la lista negra con ID {BlackListId}", id);
                throw;
            }
        }

        public async Task<BlackList> CreateAsyncSQL(BlackList blacklist)
        {
            try
            {
                string query = @"
                    INSERT INTO blacklist (reason, restrictiondate, id_client)
                    VALUES (@reason, @restrictiondate, @id_client)
                    RETURNING id;";

                blacklist.id = await _context.QuerySingleAsync<int>(query, new
                {
                    blacklist.reason,
                    blacklist.restrictiondate,
                    blacklist.id_client
                });

                return blacklist;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el registro en la lista negra: {ex.Message}");
                throw;
            }
        }

        public async Task<BlackList> CreateAsync(BlackList blacklist)
        {
            try
            {
                await _context.Set<BlackList>().AddAsync(blacklist);
                await _context.SaveChangesAsync();
                return blacklist;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el registro en la lista negra: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsyncSQL(BlackList blacklist)
        {
            try
            {
                string query = @"
                    UPDATE blacklist
                    SET reason = @reason, restrictiondate = @restrictiondate, id_client = @id_client
                    WHERE id = @id;
                    SELECT 1;";

                int rowsAffected = await _context.QuerySingleAsync<int>(query, new
                {
                    blacklist.id,
                    blacklist.reason,
                    blacklist.restrictiondate,
                    blacklist.id_client
                });

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el registro en la lista negra: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(BlackList blacklist)
        {
            try
            {
                _context.Set<BlackList>().Update(blacklist);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el registro en la lista negra: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsyncSQL(int id)
        {
            try
            {
                string query = @"DELETE FROM blacklist WHERE id = @id RETURNING 1;";
                int rowsAffected = await _context.QuerySingleAsync<int>(query, new { id });
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el registro en la lista negra: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var blacklist = await GetByIdAsync(id);
                if (blacklist == null)
                    return false;
                _context.Set<BlackList>().Remove(blacklist);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el registro en la lista negra: {ex.Message}");
                return false;
            }
        }
    }
}
