using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Entity.Context;
using Entity.DTOs;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class FormModuleData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FormModuleData> _logger;

        public FormModuleData(ApplicationDbContext context, ILogger<FormModuleData> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<FormModuleDTO>> GetAllAsyncSQL()
        {
            string query = @"
                SELECT fm.id, fm.id_forms AS FormId, f.name AS FormName,
                       fm.id_module AS ModuleId, m.name AS ModuleName
                FROM formmodule fm
                INNER JOIN forms f ON fm.id_forms = f.id
                INNER JOIN module m ON fm.id_module = m.id;";

            return await _context.QueryAsync<FormModuleDTO>(query);
        }

        public async Task<IEnumerable<FormModule>> GetAllAsync()
        {
            return await _context.Set<FormModule>()
                .Include(fm => fm.Forms)
                .Include(fm => fm.Modules)
                .ToListAsync();
        }

        public async Task<FormModuleDTO?> GetByIdAsyncSQL(int id)
        {
            try
            {
                string query = @"
                    SELECT fm.id, fm.id_forms AS FormId, f.name AS FormName,
                           fm.id_module AS ModuleId, m.name AS ModuleName
                    FROM formmodule fm
                    INNER JOIN forms f ON fm.id_forms = f.id
                    INNER JOIN module m ON fm.id_module = m.id
                    WHERE fm.id = @Id;";

                return await _context.QueryFirstOrDefaultAsync<FormModuleDTO>(query, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el FormModule con ID {FormModuleId}", id);
                throw;
            }
        }

        public async Task<FormModule?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<FormModule>()
                    .Include(fm => fm.Forms)
                    .Include(fm => fm.Modules)
                    .FirstOrDefaultAsync(fm => fm.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el FormModule con ID {FormModuleId}", id);
                throw;
            }
        }

        public async Task<FormModuleDTO> CreateAsyncSQL(FormModuleCreateDTO dto)
        {
            try
            {
                string query = @"
                    INSERT INTO formmodule (id_forms, id_module) 
                    VALUES (@FormId, @ModuleId)
                    RETURNING id;";

                var newId = await _context.QuerySingleAsync<int>(query, dto);

                return await GetByIdAsyncSQL(newId)
                    ?? throw new Exception("No se pudo recuperar el FormModule recién creado.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el FormModule: {ex.Message}");
                throw;
            }
        }

        public async Task<FormModule> CreateAsync(FormModule entity)
        {
            try
            {
                await _context.Set<FormModule>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el FormModule: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsyncSQL(int id, FormModuleCreateDTO dto)
        {
            try
            {
                string query = @"
                    UPDATE formmodule
                    SET id_forms = @FormId, id_module = @ModuleId
                    WHERE id = @Id;
                    SELECT 1;";

                int rowsAffected = await _context.QuerySingleAsync<int>(query, new
                {
                    Id = id,
                    dto.FormId,
                    dto.ModuleId
                });

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el FormModule: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(FormModule entity)
        {
            try
            {
                _context.Set<FormModule>().Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el FormModule: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsyncSQL(int id)
        {
            try
            {
                string query = @"DELETE FROM formmodule WHERE id = @Id RETURNING 1;";
                int rowsAffected = await _context.QuerySingleAsync<int>(query, new { Id = id });
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el FormModule: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await GetByIdAsync(id);
                if (entity == null) return false;

                _context.Set<FormModule>().Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el FormModule: {ex.Message}");
                return false;
            }
        }
    }
}
