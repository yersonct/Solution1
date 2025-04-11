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
    public class ModuleData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ModuleData> _logger; // Corregido: ILogger<PersonData>
        public ModuleData(ApplicationDbContext context, ILogger<ModuleData> logger) // Corregido: ILogger<UserData>
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Modules>> GetAllAsyncSQL()
        {
            string query = @"SELECT Id, Name
	                        FROM Module;";

            return await _context.QueryAsync<Modules>(query);
        }

        public async Task<IEnumerable<Modules>> GetAllAsync()
        {
            return await _context.Set<Modules>().Include(u => u.FormModules).ToListAsync();
        }

        public async Task<Modules?> GetByIdAsyncSQL(int id)
        {
            try
            {
                string query = @"SELECT Id, Name
	            FROM Module WHERE Id =@Id ";

                return await _context.QueryFirstOrDefaultAsync<Modules>(query, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener un usuario con ID {ModuleId}", id);
                throw;
            }
        }

        public async Task<Modules?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Modules>().Include(u => u.FormModules).FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener un usuario con ID {PersonId}", id);
                throw;
            }
        }

        public async Task<Modules> CreateAsyncSQL(Modules module)
        {
            try
            {
                string query = @"
                                INSERT INTO Module (Name) 
                                VALUES (@Name)
                                RETURNING Id;";


                module.Id = await _context.QuerySingleAsync<int>(query, new
                {
                    module.Name
                    
                });

                //person.Id = newId;
                return module;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<Modules> CreateAsync(Modules module)
        {
            try
            {
                await _context.Set<Modules>().AddAsync(module);
                await _context.SaveChangesAsync();
                return module;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsyncSQL(Modules module)
        {
            try
            {
                string query = @"
                                UPDATE Module    
                                SET Name = @Name
                                WHERE Id = @Id;
                                SELECT 1;";

                int rowsAffected = await _context.QuerySingleAsync<int>(query, new
                {
                    module.Id,
                    module.Name
                 

                });

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el modules: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Modules module)
        {
            try
            {
                _context.Set<Modules>().Update(module);
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
                  DELETE FROM Module WHERE id = @Id RETURNING 1;";

                int rowsAffected = await _context.QuerySingleAsync<int>(query, new { Id = id });
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el Module: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var module = await GetByIdAsync(id);
                if (module == null)
                    return false;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el modulos: {ex.Message}");
                return false;
            }
        }
    }
}
