// Data/Repository/FormModuleRepository.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Data.Interfaces;
using Entity.Context;
// using Entity.DTOs; // YA NO NECESITAMOS ESTE USING AQUÍ, PORQUE EL REPOSITORIO MANEJA ENTIDADES
using Entity.Model; // Este sí es necesario para FormModule
// using Dapper; // Si no estás usando Dapper en este repo, puedes quitarlo

namespace Data.Repository
{
    public class FormModuleRepository : IFormModuleRepository
    {
        private readonly IApplicationDbContextWithEntry _context;
        private readonly ILogger<FormModuleRepository> _logger;

        public FormModuleRepository(IApplicationDbContextWithEntry context, ILogger<FormModuleRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // CAMBIO: Ahora devuelve Task<IEnumerable<FormModule>> (entidades)
        public async Task<IEnumerable<FormModule>> GetAllAsync()
        {
            try
            {
                return await _context.Set<FormModule>()
                    .Include(fm => fm.Forms)
                    .Include(fm => fm.Modules)
                    .Where(fm => fm.Active) // Filtra solo los registros activos (true)
                    .ToListAsync(); // Directamente la lista de entidades
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los FormModule activos.");
                // Retorna una lista vacía de entidades en caso de error
                return new List<FormModule>();
            }
        }

        // CAMBIO: Ahora devuelve Task<FormModule?> (una entidad)
        public async Task<FormModule?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<FormModule>()
                    .Include(fm => fm.Forms)
                    .Include(fm => fm.Modules)
                    .Where(fm => fm.Id == id && fm.Active) // Filtra por ID y solo registros activos (true)
                    .FirstOrDefaultAsync(); // Directamente la entidad
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el FormModule activo con ID: {FormModuleId}", id);
                return null;
            }
        }

        // CAMBIO: Ahora recibe FormModule (la entidad), no DTO
        // Y devuelve la entidad que fue agregada (no DTO)
        public async Task<FormModule> AddAsync(FormModule entity)
        {
            try
            {
                _context.Set<FormModule>().Add(entity);
                await _context.SaveChangesAsync();
                return entity; // Retorna la entidad con su ID ya asignado
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el FormModule.");
                throw;
            }
        }

        // CAMBIO: Ahora recibe FormModule (la entidad), no DTO
        // La lógica de buscar y actualizar se maneja en el servicio
        public async Task<bool> UpdateAsync(FormModule entity)
        {
            try
            {
                // Asegúrate de que la entidad no esté en estado 'Added' o 'Detached' si ya existe en el contexto
                _context.Set<FormModule>().Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el FormModule con ID: {FormModuleId}.", entity.Id);
                return false;
            }
        }

        // DeleteAsync está bien, ya que recibe el ID
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.Set<FormModule>().FindAsync(id);
                if (entity == null || !entity.Active) return false;

                entity.Active = false; // Realizar la eliminación lógica
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