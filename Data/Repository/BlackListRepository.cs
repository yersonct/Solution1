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
        public BlacklistRepository(ApplicationDbContext context) : base(context) { }

        public async Task AddBlacklistAsync(BlackList entity)
        {
            var exists = await _context.Set<BlackList>().AnyAsync(b => b.id_client == entity.id_client);
            if (exists)
                throw new InvalidOperationException("Ya existe una entrada en la lista negra para este cliente.");

            await _context.Set<BlackList>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<BlackList>> GetAllWithClientAsync()
        {
            return await _context.Set<BlackList>()
                                 .Include(b => b.client) // asegúrate de que la relación esté bien configurada
                                 .ToListAsync();
        }

        public async Task<BlackList> GetByIdWithClientAsync(int id)
        {
            return await _context.Set<BlackList>()
                                 .Include(b => b.client)
                                 .FirstOrDefaultAsync(b => b.id == id);
        }

    }
}
