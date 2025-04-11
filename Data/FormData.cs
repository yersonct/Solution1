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
    public class FormData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FormData> _logger; // Corregido: ILogger<PersonData>
        public FormData(ApplicationDbContext context, ILogger<FormData> logger) // Corregido: ILogger<UserData>
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Forms>> GetAllAsyncSQL()
        {
            string query = @"SELECT Id, name,url
	                        FROM Forms;";

            return await _context.QueryAsync<Forms>(query);
        }

        public async Task<IEnumerable<Forms>> GetAllAsync()
        {
            return await _context.Set<Forms>().Include(u => u.FormModules).ToListAsync();
        }

        public async Task<Forms?> GetByIdAsyncSQL(int id)
        {
            try
            {
                string query = @"SELECT id, name,url
	            FROM Forms WHERE Id =@Id ";

                return await _context.QueryFirstOrDefaultAsync<Forms>(query, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener un usuario con ID {FormId}", id);
                throw;
            }
        }

        public async Task<Forms?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Forms>().Include(u => u.FormModules).FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener un usuario con ID {PersonId}", id);
                throw;
            }
        }

        public async Task<Forms> CreateAsyncSQL(Forms forms)
        {
            try
            {
                string query = @"
                                INSERT INTO Forms (name,url) 
                                VALUES (@Name ,@Url)
                                RETURNING Id;";


                forms.Id = await _context.QuerySingleAsync<int>(query, new
                {
                    forms.Name,
                    forms.Url
                });

                //person.Id = newId;
                return forms;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<Forms> CreateAsync(Forms forms)
        {
            try
            {
                await _context.Set<Forms>().AddAsync(forms);
                await _context.SaveChangesAsync();
                return forms;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsyncSQL(Forms forms)
        {
            try
            {
                string query = @"
                                UPDATE forms    
                                SET  name = @name ,url = @url
                                WHERE Id = @Id;
                                SELECT 1;";

                int rowsAffected = await _context.QuerySingleAsync<int>(query, new
                {
                    forms.Id,
                    forms.Name,
                    forms.Url

                });

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el modules: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Forms forms)
        {
            try
            {
                _context.Set<Forms>().Update(forms);
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
                  DELETE FROM forms WHERE id = @Id RETURNING 1;";

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
                var forms = await GetByIdAsync(id);
                if (forms == null)
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
