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
    public class MembershipsRepository : IMembershipsRepository
    {
        private readonly IApplicationDbContextWithEntry _context; // Cambiado a IApplicationDbContext
        private readonly ILogger<MembershipsRepository> _logger;

        public MembershipsRepository(IApplicationDbContextWithEntry context, ILogger<MembershipsRepository> logger) // Cambiado a IApplicationDbContext
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<MemberShips> AddAsync(MemberShips entity)
        {
            try
            {
                entity.active = true; // Establecer como activo al crear
                // Asegurar que las fechas sean UTC al crear
                if (entity.startdate.Kind != DateTimeKind.Utc)
                    entity.startdate = entity.startdate.ToUniversalTime();
                if (entity.enddate.Kind != DateTimeKind.Utc)
                    entity.enddate = entity.enddate.ToUniversalTime();

                await _context.Set<MemberShips>().AddAsync(entity); // Usar _context.Set<MemberShips>()
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
                var membershipToDelete = await _context.Set<MemberShips>().FindAsync(id); // Usar _context.Set<MemberShips>()
                if (membershipToDelete != null)
                {
                    // Asegurar que las fechas sean UTC antes de la actualización para la eliminación lógica
                    if (membershipToDelete.startdate.Kind != DateTimeKind.Utc)
                        membershipToDelete.startdate = membershipToDelete.startdate.ToUniversalTime();
                    if (membershipToDelete.enddate.Kind != DateTimeKind.Utc)
                        membershipToDelete.enddate = membershipToDelete.enddate.ToUniversalTime();

                    membershipToDelete.active = false; // Eliminación lógica
                    _context.Entry(membershipToDelete).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar (lógicamente) la membresía con ID: {MemberShipsId}", id);
                return false;
            }
        }

        public async Task<IEnumerable<MemberShips>> GetAllAsync()
        {
            try
            {
                return await _context.Set<MemberShips>() // Usar _context.Set<MemberShips>()
                    .Include(u => u.membershipsvehicles)
                    .Where(m => m.active) // Filtrar solo las activas
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las membresías activas.");
                return new List<MemberShips>();
            }
        }

        public async Task<MemberShips?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<MemberShips>() // Usar _context.Set<MemberShips>()
                    .Include(u => u.membershipsvehicles)
                    .FirstOrDefaultAsync(u => u.id == id && u.active); // Filtrar solo las activas
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la membresía activa con ID: {MemberShipsId}", id);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(MemberShips entity)
        {
            try
            {
                // Asegurar que las fechas sean UTC al actualizar
                if (entity.startdate.Kind != DateTimeKind.Utc)
                    entity.startdate = entity.startdate.ToUniversalTime();
                if (entity.enddate.Kind != DateTimeKind.Utc)
                    entity.enddate = entity.enddate.ToUniversalTime();

                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Error de concurrencia al actualizar la membresía activa con ID: {MemberShipsId}", entity.id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la membresía activa con ID: {MemberShipsId}", entity.id);
                return false;
            }
        }
    }
}