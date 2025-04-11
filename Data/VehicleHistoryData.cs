using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Entity.Context;
using Entity.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class VehicleHistoryData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<VehicleHistoryData> _logger;

        public VehicleHistoryData(ApplicationDbContext context, ILogger<VehicleHistoryData> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // ----------- Métodos con Dapper (SQL) -----------

        public async Task<IEnumerable<VehicleHistoryDTO>> GetAllAsyncSQL()
        {
            try
            {
                string query = @"SELECT id , totaltime , id_registeredvehicle, 
                                      id_typevehicle
                               FROM vehiclehistory;";
                return await _context.QueryAsync<VehicleHistoryDTO>(query);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los VehicleHistory con SQL.");
                throw;
            }
        }

        public async Task<VehicleHistoryDTO?> GetByIdAsyncSQL(int id)
        {
            try
            {
                string query = @"SELECT id , totaltime, id_registeredvehicle, 
                                      id_typevehicle 
                               FROM vehiclehistory
                               WHERE id = @Id;";
                return await _context.QueryFirstOrDefaultAsync<VehicleHistoryDTO>(query, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener VehicleHistory con ID {Id} con SQL.", id);
                throw;
            }
        }

        public async Task<int> CreateAsyncSQL(VehicleHistoryCreateDTO dto)
        {
            try
            {
                string query = @"INSERT INTO vehiclehistory (totaltime, id_registeredvehicle, id_typevehicle, id_invoice)
                               VALUES (@TotalTime, @RegisteredVehicleId, @TypeVehicleId)
                               RETURNING id;";
                return await _context.QuerySingleAsync<int>(query, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear VehicleHistory con SQL.", dto);
                throw;
            }
        }

        public async Task<bool> UpdateAsyncSQL(int id, VehicleHistoryCreateDTO dto)
        {
            try
            {
                string query = @"UPDATE vehiclehistory
                               SET totaltime = @TotalTime,
                                   id_registeredvehicle = @RegisteredVehicleId,
                                   id_typevehicle = @TypeVehicleId
                               WHERE id = @Id;
                               SELECT changes() > 0;"; // For SQLite, use changes()
                return await _context.QuerySingleAsync<bool>(query, new { Id = id, dto.TotalTime, dto.RegisteredVehicleId, dto.TypeVehicleId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar VehicleHistory con ID {Id} con SQL.", id, dto);
                throw;
            }
        }

        public async Task<bool> DeleteAsyncSQL(int id)
        {
            try
            {
                string query = @"DELETE FROM vehiclehistory WHERE id = @Id; SELECT changes() > 0;"; // For SQLite, use changes()
                return await _context.QuerySingleAsync<bool>(query, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar VehicleHistory con ID {Id} con SQL.", id);
                throw;
            }
        }

        // ----------- Métodos con LINQ (EntityFramework Core) -----------

        public async Task<IEnumerable<Entity.DTOs.VehicleHistoryDTO>> GetAllAsyncLINQ()
        {
            try
            {
                return await _context.Set<Entity.Model.VehicleHistory>()
                    .Select(vh => new Entity.DTOs.VehicleHistoryDTO
                    {
                        id = vh.id,
                        TotalTime = vh.totaltime,
                        RegisteredVehicleId = vh.id_registeredvehicle,
                        TypeVehicleId = vh.id_typevehicle
           
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los VehicleHistory con LINQ.");
                throw;
            }
        }

        public async Task<Entity.DTOs.VehicleHistoryDTO?> GetByIdAsyncLINQ(int id)
        {
            try
            {
                return await _context.Set<Entity.Model.VehicleHistory>()
                    .Where(vh => vh.id == id)
                    .Select(vh => new Entity.DTOs.VehicleHistoryDTO
                    {
                        id = vh.id,
                        TotalTime = vh.totaltime,
                        RegisteredVehicleId = vh.id_registeredvehicle,
                        TypeVehicleId = vh.id_typevehicle

     
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener VehicleHistory con ID {Id} con LINQ.", id);
                throw;
            }
        }

        public async Task<Entity.Model.VehicleHistory> CreateAsyncLINQ(VehicleHistoryCreateDTO dto)
        {
            try
            {
                var entity = new Entity.Model.VehicleHistory
                {
                    totaltime = dto.TotalTime,
                    id_registeredvehicle = dto.RegisteredVehicleId,
                    id_typevehicle = dto.TypeVehicleId
                };
                await _context.Set<Entity.Model.VehicleHistory>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear VehicleHistory con LINQ.", dto);
                throw;
            }
        }

        public async Task<bool> UpdateAsyncLINQ(int id, VehicleHistoryCreateDTO dto)
        {
            try
            {
                var entity = await _context.Set<Entity.Model.VehicleHistory>().FindAsync(id);
                if (entity == null)
                {
                    return false;
                }

                entity.totaltime = dto.TotalTime;
                entity.id_registeredvehicle = dto.RegisteredVehicleId;
                entity.id_typevehicle = dto.TypeVehicleId;

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar VehicleHistory con ID {Id} con LINQ.", id, dto);
                throw;
            }
        }

        public async Task<bool> DeleteAsyncLINQ(int id)
        {
            try
            {
                var entity = await _context.Set<Entity.Model.VehicleHistory>().FindAsync(id);
                if (entity == null)
                {
                    return false;
                }
                _context.Set<Entity.Model.VehicleHistory>().Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar VehicleHistory con ID {Id} con LINQ.", id);
                throw;
            }
        }
    }
}