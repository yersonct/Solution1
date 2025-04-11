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
    public class RatesData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RatesData> _logger; // Corregido: ILogger<RatesData>
        public RatesData(ApplicationDbContext context, ILogger<RatesData> logger) // Corregido: ILogger<UserData>
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Rates>> GetAllAsyncSQL()
        {
            string query = @"SELECT id, amount, startduration, id_typerates, endduration
	                        FROM rates;";

            return await _context.QueryAsync<Rates>(query);
        }

        public async Task<IEnumerable<Rates>> GetAllAsync()
        {
            //poner vehicleHistoryParkingRates
            return await _context.Set<Rates>().Include(u => u.id_typerates).ToListAsync();
        }

        public async Task<Rates?> GetByIdAsyncSQL(int id)
        {
            try
            {
                string query = @"SELECT id, amount, startduration, id_typerates, endduration
	            FROM rates WHERE id =@Id ";

                return await _context.QueryFirstOrDefaultAsync<Rates>(query, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener un usuario con ID {RatesId}", id);
                throw;
            }
        }

        public async Task<Rates?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Rates>().Include(u => u.id_typerates).FirstOrDefaultAsync(u => u.id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener un usuario con ID {RatesId}", id);
                throw;
            }
        }

        public async Task<Rates> CreateAsyncSQL(Rates Rates)
        {
            try
            {
                string query = @"
                                INSERT INTO rates (amount, startduration, id_typerates, endduration) 
                                VALUES (@amount, @startduration, @id_typerates, @endduration)
                                RETURNING Id;";


                Rates.id = await _context.QuerySingleAsync<int>(query, new
                {
                    Rates.id,
                    Rates.amount,
                    Rates.startduration,
                    Rates.id_typerates,
                    Rates.endduration
                });

                //Rates.Id = newId;
                return Rates;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<Rates> CreateAsync(Rates Rates)
        {
            try
            {
                await _context.Set<Rates>().AddAsync(Rates);
                await _context.SaveChangesAsync();
                return Rates;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsyncSQL(Rates Rates)
        {
            try
            {
                string query = @"
                                UPDATE rates 
                                SET amount = @amount, startduration = @startduration, id_typerates = @id_typerates, endduration = @endduration
                                WHERE Id = @Id;
                                SELECT 1;";

                int rowsAffected = await _context.QuerySingleAsync<int>(query, new
                {
                    Rates.id,
                    Rates.amount,
                    Rates.startduration,
                    Rates.id_typerates,
                    Rates.endduration


                });

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el usuario: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Rates Rates)
        {
            try
            {
                _context.Set<Rates>().Update(Rates);
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
                  DELETE FROM rates WHERE id = @Id RETURNING 1;";

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
                var Rates = await GetByIdAsync(id);
                if (Rates == null)
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
