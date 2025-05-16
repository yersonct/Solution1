using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Data.Interfaces;
using Entity.Context;
using Entity.Model;

namespace Data.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly IApplicationDbContextWithEntry _context; // Cambiado a IApplicationDbContext
        private readonly ILogger<ClientRepository> _logger;

        public ClientRepository(IApplicationDbContextWithEntry context, ILogger<ClientRepository> logger) // Cambiado a IApplicationDbContext
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Client> AddAsync(Client entity)
        {
            entity.active = true; // Establecer activo por defecto
            try
            {
                await _context.Set<Client>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar el cliente.");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var clientToDelete = await _context.Set<Client>().FindAsync(id);
                if (clientToDelete != null)
                {
                    clientToDelete.active = false; // Eliminación lógica
                    _context.Entry(clientToDelete).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar lógicamente el cliente con ID: {ClientId}", id);
                return false;
            }
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            try
            {
                return await _context.Set<Client>()
                    .Include(c => c.user)
                    .Include(c => c.vehicles)
                    .Where(c => c.active) // Filtrar solo clientes activos
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los clientes activos.");
                return new List<Client>();
            }
        }

        public async Task<Client?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Client>()
                    .Include(c => c.user)
                    .Include(c => c.vehicles)
                    .FirstOrDefaultAsync(c => c.id == id && c.active); // Filtrar por ID y activo
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el cliente activo con ID: {ClientId}", id);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(Client entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Error de concurrencia al actualizar el cliente con ID: {ClientId}", entity.id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el cliente con ID: {ClientId}", entity.id);
                return false;
            }
        }
    }
}