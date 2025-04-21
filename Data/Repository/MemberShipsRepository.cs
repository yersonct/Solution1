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
    public class MembershipsRepository : IMembershipsRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly ILogger<MembershipsRepository> _logger;

        public MembershipsRepository(ApplicationDbContext context, ILogger<MembershipsRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<MemberShips> AddAsync(MemberShips entity)
        {
            try
            {
                await _context.Set<MemberShips>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar la membresía.");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var membershipToDelete = await _context.Set<MemberShips>().FindAsync(id);
                if (membershipToDelete != null)
                {
                    _context.Set<MemberShips>().Remove(membershipToDelete);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la membresía con ID: {MemberShipsId}", id);
                return false;
            }
        }

        public async Task<IEnumerable<MemberShips>> GetAllAsync()
        {
            try
            {
                return await _context.Set<MemberShips>()
                    .Include(u => u.membershipsvehicles)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las membresías.");
                return new List<MemberShips>();
            }
        }

        public async Task<MemberShips?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<MemberShips>()
                    .Include(u => u.membershipsvehicles)
                    .FirstOrDefaultAsync(u => u.id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la membresía con ID: {MemberShipsId}", id);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(MemberShips entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Error de concurrencia al actualizar la membresía con ID: {MemberShipsId}", entity.id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la membresía con ID: {MemberShipsId}", entity.id);
                return false;
            }
        }
    }
}