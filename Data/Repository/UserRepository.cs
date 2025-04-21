using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Data.Interfaces;
using Entity.Context;
using Entity.Model;
using Dapper;

namespace Data.Repository
{
    public class UserRepository : IUserRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ApplicationDbContext context, ILogger<UserRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<User> AddAsync(User entity)
        {
            try
            {
                await _context.Set<User>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar el usuario.");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var userToDelete = await _context.Set<User>().FindAsync(id);
                if (userToDelete != null)
                {
                    _context.Set<User>().Remove(userToDelete);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el usuario con ID: {UserId}", id);
                return false;
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            try
            {
                return await _context.Set<User>()
                    .Include(u => u.person)
                    //.Include(u => u.RolUsers)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los usuarios.");
                return new List<User>();
            }
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<User>()
                    .Include(u => u.person)
                    //.Include(u => u.rolusers)
                    .FirstOrDefaultAsync(u => u.id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario con ID: {UserId}", id);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(User entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Error de concurrencia al actualizar el usuario con ID: {UserId}", entity.id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el usuario con ID: {UserId}", entity.id);
                return false;
            }
        }
    }
}