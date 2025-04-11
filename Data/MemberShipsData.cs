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
    public class MembershipsData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MembershipsData> _logger; // Corregido: ILogger<MemberShipsData>
        public MembershipsData(ApplicationDbContext context, ILogger<MembershipsData> logger) // Corregido: ILogger<UserData>
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<MemberShips>> GetAllAsyncSQL()
        {
            string query = @"SELECT id, membershiptype, startdate, enddate, active
	                        FROM memberships;";

            return await _context.QueryAsync<MemberShips>(query);
        }

        public async Task<IEnumerable<MemberShips>> GetAllAsync()
        {
            return await _context.Set<MemberShips>().Include(u => u.membershipsvehicles).ToListAsync();
        }

        public async Task<MemberShips?> GetByIdAsyncSQL(int id)
        {
            try
            {
                string query = @"SELECT id, membershiptype, startdate, enddate, active
	            FROM memberships WHERE id =@Id ";

                return await _context.QueryFirstOrDefaultAsync<MemberShips>(query, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener un usuario con ID {MemberShipsId}", id);
                throw;
            }
        }

        public async Task<MemberShips?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<MemberShips>().Include(u => u.membershipsvehicles).FirstOrDefaultAsync(u => u.id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener un usuario con ID {MemberShipsId}", id);
                throw;
            }
        }

        public async Task<MemberShips> CreateAsyncSQL(MemberShips MemberShips)
        {
            try
            {
                string query = @"
                                INSERT INTO memberships (membershiptype, startdate, enddate, active) 
                                VALUES (@MembershipType, @startdate, @EndDate, @Active)
                                RETURNING Id;";


                MemberShips.id = await _context.QuerySingleAsync<int>(query, new
                {
                    MemberShips.membershiptype,
                    MemberShips.startdate,
                    MemberShips.enddate,
                    MemberShips.active
                });

                //MemberShips.Id = newId;
                return MemberShips;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<MemberShips> CreateAsync(MemberShips MemberShips)
        {
            try
            {
                await _context.Set<MemberShips>().AddAsync(MemberShips);
                await _context.SaveChangesAsync();
                return MemberShips;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsyncSQL(MemberShips MemberShips)
        {
            try
            {
                string query = @"
                                UPDATE memberships 
                                SET membershiptype = @MembershipType, startdate = @StartDate, enddate = @EndDate, active = @Active
                                WHERE Id = @Id;
                                SELECT 1;";

                int rowsAffected = await _context.QuerySingleAsync<int>(query, new
                {
                    MemberShips.id,
                    MemberShips.membershiptype,
                    MemberShips.startdate,
                    MemberShips.enddate,
                    MemberShips.active
            

                });

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el usuario: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(MemberShips MemberShips)
        {
            try
            {
                _context.Set<MemberShips>().Update(MemberShips);
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
                  DELETE FROM MemberShips WHERE id = @Id RETURNING 1;";

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
                var MemberShips = await GetByIdAsync(id);
                if (MemberShips == null)
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
