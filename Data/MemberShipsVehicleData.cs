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
    public class MembershipsVehicleData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MembershipsVehicleData> _logger;

        public MembershipsVehicleData(ApplicationDbContext context, ILogger<MembershipsVehicleData> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> CanConnectAsync()
        {
            try
            {
                return await _context.Database.CanConnectAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar conexión a la base de datos.");
                return false;
            }
        }

        public async Task<IEnumerable<MembershipsVehicle>> GetAllAsyncSQL()
        {
            string query = @"
                SELECT mv.id, mv.id_vehicle, mv.id_memberships
                FROM membershipsvehicle mv;
            ";

            return await _context.QueryAsync<MembershipsVehicle>(query);
        }

        public async Task<IEnumerable<MembershipsVehicle>> GetAllAsync()
        {
            return await _context.Set<MembershipsVehicle>()
                                 .Include(mv => mv.vehicle)
                                 .Include(mv => mv.memberships)
                                 .ToListAsync();
        }

        public async Task<MembershipsVehicle?> GetByIdAsyncSQL(int id)
        {
            try
            {
                string query = @"
                    SELECT id, id_vehicle, id_memberships
                    FROM membershipsvehicle
                    WHERE id = @id;
                ";
                return await _context.QueryFirstOrDefaultAsync<MembershipsVehicle>(query, new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el registro con ID {MembershipsVehicleId}", id);
                throw;
            }
        }

        public async Task<MembershipsVehicle?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<MembershipsVehicle>()
                                     .Include(mv => mv.vehicle)
                                     .Include(mv => mv.memberships)
                                     .FirstOrDefaultAsync(mv => mv.id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el registro con ID {MembershipsVehicleId}", id);
                throw;
            }
        }

        public async Task<MembershipsVehicle> CreateAsyncSQL(MembershipsVehicle mv)
        {
            try
            {
                string query = @"
                    INSERT INTO membershipsvehicle (id_vehicle, id_memberships)
                    VALUES (@id_vehicle, @id_memberships)
                    RETURNING id;
                ";

                mv.id = await _context.QuerySingleAsync<int>(query, new
                {
                    mv.id_vehicle,
                    mv.id_memberships
                });

                return mv;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el registro: {ex.Message}");
                throw;
            }
        }

        public async Task<MembershipsVehicle> CreateAsync(MembershipsVehicle mv)
        {
            try
            {
                await _context.Set<MembershipsVehicle>().AddAsync(mv);
                await _context.SaveChangesAsync();
                return mv;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el registro: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsyncSQL(MembershipsVehicle mv)
        {
            try
            {
                string query = @"
                    UPDATE membershipsvehicle
                    SET id_vehicle = @id_vehicle, id_memberships = @id_memberships
                    WHERE id = @id;
                    SELECT 1;
                ";

                int result = await _context.QuerySingleAsync<int>(query, new
                {
                    mv.id,
                    mv.id_vehicle,
                    mv.id_memberships
                });

                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el registro: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(MembershipsVehicle mv)
        {
            try
            {
                _context.Set<MembershipsVehicle>().Update(mv);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el registro: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsyncSQL(int id)
        {
            try
            {
                string query = @"DELETE FROM membershipsvehicle WHERE id = @id RETURNING 1;";
                int result = await _context.QuerySingleAsync<int>(query, new { id });
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el registro: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var mv = await GetByIdAsync(id);
                if (mv == null)
                    return false;

                _context.Set<MembershipsVehicle>().Remove(mv);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el registro: {ex.Message}");
                return false;
            }
        }
    }
}
