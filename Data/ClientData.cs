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
    public class ClientData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ClientData> _logger;

        public ClientData(ApplicationDbContext context, ILogger<ClientData> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Método auxiliar para verificar la conexión con la base de datos
        public async Task<bool> CanConnectAsync()
        {
            try
            {
                return await _context.Database.CanConnectAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar la conexión a la base de datos.");
                return false;
            }
        }

        public async Task<IEnumerable<Client>> GetAllAsyncSQL()
        {
            string query = @"SELECT id, id_user, name FROM client;";
            return await _context.QueryAsync<Client>(query);
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            return await _context.Set<Client>()
                                 .Include(c => c.user)
                                 .Include(c => c.vehicles)
                                 .ToListAsync();
        }

        public async Task<Client?> GetByIdAsyncSQL(int id)
        {
            try
            {
                string query = @"SELECT id, id_user, name FROM client WHERE id = @id;";
                return await _context.QueryFirstOrDefaultAsync<Client>(query, new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el cliente con ID {ClientId}", id);
                throw;
            }
        }

        public async Task<Client?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Client>()
                                     .Include(c => c.user)
                                     .Include(c => c.vehicles)
                                     .FirstOrDefaultAsync(c => c.id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el cliente con ID {ClientId}", id);
                throw;
            }
        }

        public async Task<Client> CreateAsyncSQL(Client client)
        {
            try
            {
                string query = @"
                    INSERT INTO client (id_user, name)
                    VALUES (@id_user, @name)
                    RETURNING id;";

                client.id = await _context.QuerySingleAsync<int>(query, new
                {
                    client.id_user,
                    client.name
                });

                return client;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el cliente: {ex.Message}");
                throw;
            }
        }

        public async Task<Client> CreateAsync(Client client)
        {
            try
            {
                await _context.Set<Client>().AddAsync(client);
                await _context.SaveChangesAsync();
                return client;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el cliente: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsyncSQL(Client client)
        {
            try
            {
                string query = @"
                    UPDATE client
                    SET id_user = @id_user, name = @name
                    WHERE id = @id;
                    SELECT 1;";

                int rowsAffected = await _context.QuerySingleAsync<int>(query, new
                {
                    client.id,
                    client.id_user,
                    client.name
                });

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el cliente: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Client client)
        {
            try
            {
                _context.Set<Client>().Update(client);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el cliente: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsyncSQL(int id)
        {
            try
            {
                string query = "DELETE FROM client WHERE id = @id RETURNING 1;";
                int rowsAffected = await _context.QuerySingleAsync<int>(query, new { id });
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el cliente: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var client = await GetByIdAsync(id);
                if (client == null)
                    return false;

                _context.Client.Remove(client);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el cliente: {ex.Message}");
                return false;
            }
        }
    }
}
