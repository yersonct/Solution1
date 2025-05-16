using Data.Interfaces;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly IApplicationDbContext _context; // Cambiado a IApplicationDbContext
        private readonly ILogger<PersonRepository> _logger;

        public PersonRepository(IApplicationDbContext context, ILogger<PersonRepository> logger) // Cambiado a IApplicationDbContext
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Person> AddAsync(Person entity)
        {
            try
            {
                await _context.Set<Person>().AddAsync(entity); // Usar _context.Set<Person>()
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar la persona.");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var personToDelete = await _context.Set<Person>().FindAsync(id); // Usar _context.Set<Person>()
                if (personToDelete != null)
                {
                    personToDelete.active = false; // Implementación del borrado lógico
                    _context.Entry(personToDelete).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar (lógicamente) la persona con ID: {PersonId}", id);
                return false;
            }
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            try
            {
                return await _context.Set<Person>().Include(u => u.user).Where(p => p.active).ToListAsync(); // Solo trae los activos // Usar _context.Set<Person>()
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las personas activas.");
                return new List<Person>();
            }
        }

        public async Task<Person?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Person>().Include(u => u.user).FirstOrDefaultAsync(u => u.id == id && u.active); // Solo trae los activos // Usar _context.Set<Person>()
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la persona activa con ID: {PersonId}", id);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(Person entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Error de concurrencia al actualizar la persona con ID: {PersonId}", entity.id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la persona con ID: {PersonId}", entity.id);
                return false;
            }
        }

        public async Task<bool> PersonExistsAsync(string document, string email)
        {
            return await _context.Set<Person>().AnyAsync(p => p.document == document || p.email == email); // Usar _context.Set<Person>()
        }
    }
}