using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class TypeVehicleData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TypeVehicleData> _logger;

        public TypeVehicleData(ApplicationDbContext context, ILogger<TypeVehicleData> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Obtener todos los TypeVehicles
        public async Task<IEnumerable<TypeVehicle>> GetAllAsyncSQL()
        {
            string query = @"SELECT id, name FROM typevehicles;";
            return await _context.QueryAsync<TypeVehicle>(query);
        }

        // Obtener todos los TypeVehicles usando EF
        public async Task<IEnumerable<TypeVehicle>> GetAllAsync()
        {
            return await _context.Set<TypeVehicle>().ToListAsync();
        }

        // Obtener un TypeVehicle por ID (con SQL)
        public async Task<TypeVehicle?> GetByIdAsyncSQL(int id)
        {
            try
            {
                string query = @"SELECT id, name FROM typevehicles WHERE id = @Id";
                return await _context.QueryFirstOrDefaultAsync<TypeVehicle>(query, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener TypeVehicle con ID {TypeVehicleId}", id);
                throw;
            }
        }

        // Obtener un TypeVehicle por ID (con EF)
        public async Task<TypeVehicle?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<TypeVehicle>().FirstOrDefaultAsync(u => u.id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener TypeVehicle con ID {TypeVehicleId}", id);
                throw;
            }
        }

        // Crear un nuevo TypeVehicle (con SQL)
        public async Task<TypeVehicle> CreateAsyncSQL(TypeVehicle typeVehicle)
        {
            try
            {
                string query = @"
                    INSERT INTO typevehicles (name) 
                    VALUES (@name) 
                    RETURNING id;";

                typeVehicle.id = await _context.QuerySingleAsync<int>(query, new
                {
                    typeVehicle.name
                });

                return typeVehicle;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear TypeVehicle: {ex.Message}");
                throw;
            }
        }

        // Crear un nuevo TypeVehicle (con EF)
        public async Task<TypeVehicle> CreateAsync(TypeVehicle typeVehicle)
        {
            try
            {
                await _context.Set<TypeVehicle>().AddAsync(typeVehicle);
                await _context.SaveChangesAsync();
                return typeVehicle;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear TypeVehicle: {ex.Message}");
                throw;
            }
        }

        // Actualizar un TypeVehicle (con SQL)
        public async Task<bool> UpdateAsyncSQL(TypeVehicle typeVehicle)
        {
            try
            {
                string query = @"
                    UPDATE typevehicles 
                    SET name = @name
                    WHERE id = @Id;
                    SELECT 1;";

                int rowsAffected = await _context.QuerySingleAsync<int>(query, new
                {
                    typeVehicle.id,
                    typeVehicle.name
                });

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar TypeVehicle: {ex.Message}");
                return false;
            }
        }

        // Actualizar un TypeVehicle (con EF)
        public async Task<bool> UpdateAsync(TypeVehicle typeVehicle)
        {
            try
            {
                _context.Set<TypeVehicle>().Update(typeVehicle);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar TypeVehicle: {ex.Message}");
                return false;
            }
        }

        // Eliminar un TypeVehicle (con SQL)
        public async Task<bool> DeleteAsyncSQL(int id)
        {
            try
            {
                string query = @"DELETE FROM typevehicles WHERE id = @Id RETURNING 1;";
                int rowsAffected = await _context.QuerySingleAsync<int>(query, new { Id = id });
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar TypeVehicle: {ex.Message}");
                return false;
            }
        }

        // Eliminar un TypeVehicle (con EF)
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var typeVehicle = await GetByIdAsync(id);
                if (typeVehicle == null)
                    return false;

                _context.Set<TypeVehicle>().Remove(typeVehicle);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar TypeVehicle: {ex.Message}");
                return false;
            }
        }
    }
}
