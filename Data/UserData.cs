    using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class UserData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserData> _logger;

        public UserData(ApplicationDbContext context, ILogger<UserData> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<User>> GetAllAsyncSQL()
        {
            string query = @"SELECT id, username, password, id_person FROM ""user"";";
            return await _context.QueryAsync<User>(query);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Set<User>()
                .Include(u => u.person)
                //.Include(u => u.RolUsers)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsyncSQL(int id)
        {
            try
            {
                string query = @"SELECT id, username, password, id_person FROM ""user"" WHERE id = @id;";
                return await _context.QueryFirstOrDefaultAsync<User>(query, new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener un usuario con ID {UserId}", id);
                throw;
            }
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<User>()
                    .Include(u => u.person)
                    //.Include(u => u.rolusers)
                    .FirstOrDefaultAsync(u => u.id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener un usuario con ID {UserId}", id);
                throw;
            }
        }

        public async Task<User> CreateAsyncSQL(User user)
        {
            try
            {
                string query = @"
                    INSERT INTO ""user"" (username, password, id_person)
                    VALUES (@username, @password, @id_person)
                    RETURNING id;";

                user.id = await _context.QuerySingleAsync<int>(query, new
                {
                    user.username,
                    user.password,
                    user.id_person
                });

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el usuario");
                throw;
            }
        }

        public async Task<User> CreateAsync(User user)
        {
            try
            {
                await _context.Set<User>().AddAsync(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el usuario");
                throw;
            }
        }

        public async Task<bool> UpdateAsyncSQL(User user)
        {
            try
            {
                string query = @"
                    UPDATE ""user""
                    SET username = @username,
                        password = @password,
                        id_person = @id_person
                    WHERE id = @id;
                    SELECT 1;";

                int rowsAffected = await _context.QuerySingleAsync<int>(query, new
                {
                    user.id,
                    user.username,
                    user.password,
                    user.id_person
                });

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el usuario");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(User user)
        {
            try
            {
                _context.Set<User>().Update(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el usuario");
                return false;
            }
        }

        public async Task<bool> DeleteAsyncSQL(int id)
        {
            try
            {
                string query = @"DELETE FROM ""user"" WHERE id = @id RETURNING 1;";
                int rowsAffected = await _context.QuerySingleAsync<int>(query, new { id });
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el usuario");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var user = await GetByIdAsync(id);
                if (user == null)
                    return false;

                _context.Set<User>().Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el usuario");
                return false;
            }
        }
    }
}
