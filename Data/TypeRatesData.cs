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
    public class TypeRatesData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TypeRatesData> _logger; // Corregido: ILogger<TypeRatesData>
        public TypeRatesData(ApplicationDbContext context, ILogger<TypeRatesData> logger) // Corregido: ILogger<UserData>
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<TypeRates>> GetAllAsyncSQL()
        {
            string query = @"SELECT id, name, price FROM typerates;";

            return await _context.QueryAsync<TypeRates>(query);
        }

        public async Task<IEnumerable<TypeRates>> GetAllAsync()
        {
            return await _context.Set<TypeRates>().Include(u => u.rates).ToListAsync();
        }

        public async Task<TypeRates?> GetByIdAsyncSQL(int id)
        {
            try
            {
                string query = @"SELECT id, name,price
	            FROM typerates WHERE id =@Id ";

                return await _context.QueryFirstOrDefaultAsync<TypeRates>(query, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener un usuario con ID {TypeRatesId}", id);
                throw;
            }
        }

        public async Task<TypeRates?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<TypeRates>().Include(u => u.rates).FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener un usuario con ID {TypeRatesId}", id);
                throw;
            }
        }

        public async Task<TypeRates> CreateAsyncSQL(TypeRates TypeRates)
        {
            try
            {
                string query = @"
                                INSERT INTO typerates (name, price) 
                                VALUES (@Name, @Price)
                                RETURNING Id;";


                TypeRates.Id = await _context.QuerySingleAsync<int>(query, new
                {
                    TypeRates.Name,
                    TypeRates.Price,
             
                });

                //TypeRates.Id = newId;
                return TypeRates;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<TypeRates> CreateAsync(TypeRates TypeRates)
        {
            try
            {
                await _context.Set<TypeRates>().AddAsync(TypeRates);
                await _context.SaveChangesAsync();
                return TypeRates;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsyncSQL(TypeRates TypeRates)
        {
            try
            {
                //poner  el ratesid
                string query = @"
                                UPDATE TypeRates 
                                SET name = @Name, price = @Price
                                WHERE id = @Id;
                                SELECT 1;";

                int rowsAffected = await _context.QuerySingleAsync<int>(query, new
                {
                    TypeRates.Name,
                    TypeRates.Price,


                });

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el usuario: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(TypeRates TypeRates)
        {
            try
            {
                _context.Set<TypeRates>().Update(TypeRates);
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
                  DELETE FROM TypeRates WHERE id = @Id RETURNING 1;";

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
                var TypeRates = await GetByIdAsync(id);
                if (TypeRates == null)
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
