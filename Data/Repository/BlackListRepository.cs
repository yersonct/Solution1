using Data.Interfaces;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class BlacklistRepository : Repository<BlackList>, IBlacklistRepository
    {
        private readonly IApplicationDbContext _context; // Cambiado a IApplicationDbContext

        public BlacklistRepository(IApplicationDbContext context) : base(context) // Cambiado a IApplicationDbContext
        {
            _context = context;
        }

        public async Task AddBlacklistAsync(BlackList entity)
        {
            var exists = await _context.Set<BlackList>().AnyAsync(b => b.id_client == entity.id_client && b.active);
            if (exists)
                throw new InvalidOperationException("Ya existe una entrada activa en la lista negra para este cliente.");

            entity.active = true;
            await _context.Set<BlackList>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<BlackList>> GetAllAsync()
        {
            return await _context.Set<BlackList>()
                                     .Where(b => b.active)
                                     .ToListAsync();
        }

        public async Task<BlackList> GetByIdAsync(int id)
        {
            return await _context.Set<BlackList>()
                                     .FirstOrDefaultAsync(b => b.id == id && b.active);
        }

        public async Task<IEnumerable<BlackList>> GetAllWithClientAsync()
        {
            return await _context.Set<BlackList>()
                                     .Include(b => b.client)
                                     .Where(b => b.active)
                                     .ToListAsync();
        }

        public async Task<BlackList> GetByIdWithClientAsync(int id)
        {
            return await _context.Set<BlackList>()
                                     .Include(b => b.client)
                                     .FirstOrDefaultAsync(b => b.id == id && b.active);
        }

        public void Update(BlackList entity)
        {
            _context.Set<BlackList>().Update(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(BlackList entityToDelete)
        {
            var existingEntity = await _context.Set<BlackList>().FindAsync(entityToDelete.id);
            if (existingEntity != null)
            {
                if (existingEntity.restrictiondate.Kind != DateTimeKind.Utc)
                {
                    existingEntity.restrictiondate = existingEntity.restrictiondate.ToUniversalTime();
                }
                existingEntity.active = false; // Eliminación lógica
                _context.Set<BlackList>().Update(existingEntity);
                await _context.SaveChangesAsync();
            }
        }
    }
}