using Data.Interfaces;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;

namespace Data.Repository
{
    public class UserRepository : IUserRepository, IloginRepository
    {
        private readonly IApplicationDbContextWithEntry _context; // Cambiado a IApplicationDbContext
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(IApplicationDbContextWithEntry context, ILogger<UserRepository> logger) // Cambiado a IApplicationDbContext
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<User> AddAsync(User entity)
        {
            try
            {
                // Trunca el nombre de usuario antes de agregarlo al contexto
                if (entity.username.Length > 20)
                {
                    entity.username = entity.username.Substring(0, 20);
                    _logger.LogWarning("AddAsync: Nombre de usuario truncado a: {Username}", entity.username);
                }

                // Trunca la contraseña antes de agregarla al contexto
                //if (entity.password.Length > 20)
                //{
                //    entity.password = entity.password.Substring(0, 20);
                //    _logger.LogWarning("AddAsync: Contraseña truncada a: {Password}", entity.password);
                //}

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
                    userToDelete.active = false; // Asumiendo que tienes un campo 'active'
                    _context.Entry(userToDelete).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar (lógicamente) el usuario con ID: {UserId}", id);
                return false;
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            try
            {
                return await _context.Set<User>()
                    .Include(u => u.person)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los usuarios.");
                return new List<User>();
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await GetAllAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<User>()
                    .Include(u => u.person)
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
                // Trunca el nombre de usuario antes de actualizar la entidad
                if (entity.username.Length > 20)
                {
                    entity.username = entity.username.Substring(0, 20);
                    _logger.LogWarning("UpdateAsync: Nombre de usuario truncado a: {Username}", entity.username);
                }

                // Trunca la contraseña antes de actualizar la entidad
                //if (entity.password.Length > 20)
                //{
                //    entity.password = entity.password.Substring(0, 20);
                //    _logger.LogWarning("UpdateAsync: Contraseña truncada a: {Password}", entity.password);
                //}

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

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Set<User>().FirstOrDefaultAsync(u => u.username == username);
        }
    }
}