using System;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class CamaraData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CamaraData> _logger;

        public CamaraData(ApplicationDbContext context, ILogger<CamaraData> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Camara>> GetAllAsyncSQL()
        {
            string query = @"SELECT id, name, nightvisioninfrared, highresolution, infraredlighting, optimizedangleofvision, highshutterspeed
                             FROM camara;";

            return await _context.QueryAsync<Camara>(query);
        }

        public async Task<IEnumerable<Camara>> GetAllAsync()
        {
            return await _context.Set<Camara>().ToListAsync();
        }

        public async Task<Camara?> GetByIdAsyncSQL(int id)
        {
            try
            {
                string query = @"SELECT id, name, nightvisioninfrared, highresolution, infraredlighting, optimizedangleofvision, highshutterspeed
                                 FROM camara WHERE id = @id";

                return await _context.QueryFirstOrDefaultAsync<Camara>(query, new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener una cámara con ID {id}", id);
                throw;
            }
        }

        public async Task<Camara?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Camara>().FirstOrDefaultAsync(u => u.id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener una cámara con ID {id}", id);
                throw;
            }
        }

        public async Task<Camara> CreateAsyncSQL(Camara camara)
        {
            try
            {
                string query = @"
                    INSERT INTO camara (name, nightvisioninfrared, highresolution, infraredlighting, optimizedangleofvision, highshutterspeed) 
                    VALUES (@name, @nightvisioninfrared, @highresolution, @infraredlighting, @optimizedangleofvision, @highshutterspeed)
                    RETURNING id;";

                camara.id = await _context.QuerySingleAsync<int>(query, camara);
                return camara;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la cámara");
                throw;
            }
        }

        public async Task<Camara> CreateAsync(Camara camara)
        {
            try
            {
                await _context.Set<Camara>().AddAsync(camara);
                await _context.SaveChangesAsync();
                return camara;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la cámara");
                throw;
            }
        }

        public async Task<bool> UpdateAsyncSQL(Camara camara)
        {
            try
            {
                string query = @"
                    UPDATE camara 
                    SET name = @name,
                        nightvisioninfrared = @nightvisioninfrared,
                        highresolution = @highresolution,
                        infraredlighting = @infraredlighting,
                        optimizedangleofvision = @optimizedangleofvision,
                        highshutterspeed = @highshutterspeed
                    WHERE id = @id;
                    SELECT 1;";

                int rowsAffected = await _context.QuerySingleAsync<int>(query, camara);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la cámara");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Camara camara)
        {
            try
            {
                _context.Set<Camara>().Update(camara);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la cámara");
                return false;
            }
        }

        public async Task<bool> DeleteAsyncSQL(int id)
        {
            try
            {
                string query = @"DELETE FROM camara WHERE id = @id RETURNING 1;";
                int rowsAffected = await _context.QuerySingleAsync<int>(query, new { id });
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la cámara");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var camara = await GetByIdAsync(id);
                if (camara == null)
                    return false;

                _context.Set<Camara>().Remove(camara);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la cámara");
                return false;
            }
        }
    }
}
