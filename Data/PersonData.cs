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
    public class PersonData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PersonData> _logger; // Corregido: ILogger<PersonData>
        public PersonData(ApplicationDbContext context, ILogger<PersonData> logger) // Corregido: ILogger<UserData>
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Person>> GetAllAsyncSQL()
        {
            string query = @"SELECT id, name, document, phone, lastname, email,active
	                        FROM Person;";

            return await _context.QueryAsync<Person>(query);
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            return await _context.Set<Person>().Include(u => u.user).ToListAsync();
        }

        public async Task<Person?> GetByIdAsyncSQL(int id)
        {
            try
            {
                string query = @"SELECT id, name, document, phone, lastname, email,active
	            FROM person WHERE Id =@Id ";

                return await _context.QueryFirstOrDefaultAsync<Person>(query, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener un usuario con ID {PersonId}", id);
                throw;
            }
        }

        public async Task<Person?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Person>().Include(u => u.user).FirstOrDefaultAsync(u => u.id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener un usuario con ID {PersonId}", id);
                throw;
            }
        }

        public async Task<Person> CreateAsyncSQL(Person person)
        {
            try
            {
                string query = @"
                                INSERT INTO person (name, document, phone, lastname, email, active) 
                                VALUES (@name, @document, @phone, @lastname, @email, true)
                                RETURNING Id;";


                person.id = await _context.QuerySingleAsync<int>(query, new
                {
                    person.name,
                    person.document,
                    person.phone,
                    person.lastname,
                    person.email
                });

                //person.Id = newId;
                return person;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<Person> CreateAsync(Person person)
        {
            try
            {
                await _context.Set<Person>().AddAsync(person);
                await _context.SaveChangesAsync();
                return person;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsyncSQL(Person person)
        {
            try
            {
                string query = @"
                                UPDATE person 
                                SET name = @name, document = @document, phone = @phone, lastname = @lastname, email = @email, active = @active
                                WHERE id = @Id;
                                SELECT 1;";

                int rowsAffected = await _context.QuerySingleAsync<int>(query, new
                {
                    person.id,
                    person.name,
                    person.document,
                    person.phone,
                    person.lastname,
                    person.email,
                    person.active
                 
                });

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el usuario: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Person person)
        {
            try
            {
                _context.Set<Person>().Update(person);
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
                  DELETE FROM person WHERE id = @Id RETURNING 1;";

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
                var person = await GetByIdAsync(id);
                if (person == null)
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
