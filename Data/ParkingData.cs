using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Dapper;

namespace Data
{
    public class ParkingData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ParkingData> _logger;

        public ParkingData(ApplicationDbContext context, ILogger<ParkingData> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Obtener todos los parkings usando sentencia SQL
        public async Task<IEnumerable<Parking>> GetAllAsyncSQL()
        {
            try
            {
                string query = @"SELECT p.id, p.name, p.location, p.hability, p.id_camara
                                 FROM ""Parking"" p;";
                var parkings = await _context.Database.GetDbConnection().QueryAsync<Parking>(query);
                return parkings;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los parkings usando SQL.");
                throw;
            }
        }

        // Obtener parking por ID usando sentencia SQL
        public async Task<Parking?> GetByIdAsyncSQL(int id)
        {
            try
            {
                string query = @"SELECT p.id, p.name, p.location, p.hability, p.id_camara
                                 FROM ""parking"" p
                                 WHERE p.id = @id;";
                return await _context.Database.GetDbConnection().QueryFirstOrDefaultAsync<Parking>(query, new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el parking con ID {ParkingId} usando SQL", id);
                throw;
            }
        }

        // Crear un nuevo parking usando sentencia SQL
        public async Task<Parking> CreateAsyncSQL(Parking parking)
        {
            try
            {
                string query = @"
                    INSERT INTO ""parking"" (name, location, hability, id_camara)
                    VALUES (@name, @location, @hability, @id_camara)
                    RETURNING id;";

                parking.id = await _context.Database.GetDbConnection().QuerySingleAsync<int>(query, new
                {
                    parking.name,
                    parking.location,
                    parking.hability,
                    parking.id_camara
                });

                return parking;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el parking usando SQL.");
                throw;
            }
        }

        // Actualizar parking usando sentencia SQL
        public async Task<bool> UpdateAsyncSQL(Parking parking)
        {
            try
            {
                string query = @"
                    UPDATE ""parking"" 
                    SET name = @name, location = @location, hability = @hability, id_camara = @id_camara
                    WHERE id = @id
                    RETURNING 1;";

                int rowsAffected = await _context.Database.GetDbConnection().QuerySingleAsync<int>(query, new
                {
                    parking.id,
                    parking.name,
                    parking.location,
                    parking.hability,
                    parking.id_camara
                });

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el parking usando SQL.");
                return false;
            }
        }

        // Eliminar parking por ID usando sentencia SQL
        public async Task<bool> DeleteAsyncSQL(int id)
        {
            try
            {
                string query = @"DELETE FROM ""parking"" WHERE id = @id RETURNING 1;";
                int rowsAffected = await _context.Database.GetDbConnection().QuerySingleAsync<int>(query, new { id });
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el parking con ID {ParkingId} usando SQL", id);
                return false;
            }
        }

        // Obtener todos los parkings usando LINQ
        public async Task<IEnumerable<Parking>> GetAllAsync()
        {
            try
            {
                return await _context.Parkings
                    .Include(p => p.Camaras) // Relación con Camara
                    .Include(p => p.VehicleHistoryParkingRates) // Relación con VehicleHistoryParkingRates
                    .Select(p => new Parking
                    {
                        id = p.id,
                        name = p.name,
                        location = p.location,
                        hability = p.hability,
                        id_camara = p.id_camara // Asegúrate de incluir este campo
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los parkings usando LINQ.");
                throw;
            }
        }

        // Obtener parking por ID usando LINQ
        public async Task<Parking?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Parkings
                    .Include(p => p.Camaras)
                    .Include(p => p.VehicleHistoryParkingRates)
                    .FirstOrDefaultAsync(p => p.id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el parking con ID {ParkingId} usando LINQ", id);
                throw;
            }
        }

        // Crear un nuevo parking usando LINQ
        public async Task<Parking> CreateAsync(Parking parking)
        {
            try
            {
                await _context.Parkings.AddAsync(parking);
                await _context.SaveChangesAsync();
                return parking;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el parking usando LINQ.");
                throw;
            }
        }

        // Actualizar parking usando LINQ
        public async Task<bool> UpdateAsync(Parking parking)
        {
            try
            {
                _context.Parkings.Update(parking);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el parking usando LINQ.");
                return false;
            }
        }

        // Eliminar parking usando LINQ
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var parking = await GetByIdAsync(id);
                if (parking == null)
                    return false;

                _context.Parkings.Remove(parking);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el parking con ID {ParkingId} usando LINQ", id);
                return false;
            }
        }
    }
}
