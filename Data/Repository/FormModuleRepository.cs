using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Data.Interfaces;
using Entity.Context;
using Entity.DTOs;
using Entity.Model;
using Dapper;

namespace Data.Repository
{
    public class FormModuleRepository : IFormModuleRepository
    {
        private readonly IApplicationDbContext _context; // Cambiado a IApplicationDbContext
        private readonly ILogger<FormModuleRepository> _logger;

        public FormModuleRepository(IApplicationDbContext context, ILogger<FormModuleRepository> logger) // Cambiado a IApplicationDbContext
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<FormModuleDTO>> GetAllAsync()
        {
            try
            {
                return await _context.Set<FormModule>()
                    .Include(fm => fm.Forms)
                    .Include(fm => fm.Modules)
                    .Where(fm => fm.active) // Filtra solo los registros activos (true)
                    .Select(fm => new FormModuleDTO
                    {
                        Id = fm.id,
                        id_forms = fm.id_forms,
                        FormName = fm.Forms.name,
                        id_module = fm.id_module,
                        ModuleName = fm.Modules.name,
                        active = fm.active // Incluimos la propiedad Active en el DTO
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los FormModule activos.");
                return new List<FormModuleDTO>();
            }
        }

        public async Task<FormModuleDTO?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<FormModule>()
                    .Include(fm => fm.Forms)
                    .Include(fm => fm.Modules)
                    .Where(fm => fm.id == id && fm.active) // Filtra por ID y solo registros activos (true)
                    .Select(fm => new FormModuleDTO
                    {
                        Id = fm.id,
                        id_forms = fm.id_forms,
                        FormName = fm.Forms.name,
                        id_module = fm.id_module,
                        ModuleName = fm.Modules.name,
                        active = fm.active // Incluimos la propiedad Active en el DTO
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el FormModule activo con ID: {FormModuleId}", id);
                return null;
            }
        }

        public async Task<FormModuleDTO> AddAsync(FormModuleCreateDTO dto)
        {
            try
            {
                var entity = new FormModule
                {
                    id_forms = dto.id_forms,
                    id_module = dto.id_module,
                    active = true // Establecer como activo al crear
                };

                _context.Set<FormModule>().Add(entity);
                await _context.SaveChangesAsync();

                return await GetByIdAsync(entity.id)
                       ?? throw new Exception("No se pudo recuperar el FormModule recién creado.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el FormModule.");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(int id, FormModuleCreateDTO dto)
        {
            try
            {
                var entity = await _context.Set<FormModule>().FindAsync(id);
                if (entity == null || !entity.active) return false;

                entity.id_forms = dto.id_forms;
                entity.id_module = dto.id_module;

                _context.Set<FormModule>().Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el FormModule con ID: {FormModuleId}.", id);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.Set<FormModule>().FindAsync(id);
                if (entity == null || !entity.active) return false;

                entity.active = false; // Realizar la eliminación lógica
                _context.Set<FormModule>().Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el FormModule con ID: {FormModuleId}.", id);
                return false;
            }
        }
    }
}