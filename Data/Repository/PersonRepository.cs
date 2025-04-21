using Data.Interfaces;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class PersonRepository : IPersonRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly ILogger<PersonRepository> _logger;

        public PersonRepository(ApplicationDbContext context, ILogger<PersonRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Person> AddAsync(Person entity)
        {
            try
            {
                await _context.Set<Person>().AddAsync(entity);
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
                var personToDelete = await _context.Set<Person>().FindAsync(id);
                if (personToDelete != null)
                {
                    _context.Set<Person>().Remove(personToDelete);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la persona con ID: {PersonId}", id);
                return false;
            }
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            try
            {
                return await _context.Set<Person>().Include(u => u.user).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las personas.");
                return new List<Person>();
            }
        }

        public async Task<Person?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Person>().Include(u => u.user).FirstOrDefaultAsync(u => u.id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la persona con ID: {PersonId}", id);
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
    }
}
