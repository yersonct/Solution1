using Dapper;
using Data.Interfaces;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;

namespace Data.Repository
{
    public class CamaraRepository : ICamaraRepository
    {
        private readonly IApplicationDbContext _context; // Cambiado a IApplicationDbContext
        private readonly ILogger<CamaraRepository> _logger;

        public CamaraRepository(IApplicationDbContext context, ILogger<CamaraRepository> logger) // Cambiado a IApplicationDbContext
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Camara>> GetAllAsync()
        {
            return await _context.Set<Camara>().ToListAsync();
        }

        public async Task<Camara?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Camara>().FirstOrDefaultAsync(c => c.id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener cámara con ID {id}", id);
                throw;
            }
        }

        public async Task<Camara> CreateAsync(Camara Camara)
        {
            try
            {
                await _context.Set<Camara>().AddAsync(Camara);
                await _context.SaveChangesAsync();
                return Camara;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear cámara");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Camara camara)
        {
            try
            {
                var trackedEntity = await _context.Set<Camara>() // Usamos _context.Set<Camara>()
                    .FirstOrDefaultAsync(c => c.id == camara.id);

                if (trackedEntity != null)
                {
                    _context.Entry(trackedEntity).CurrentValues.SetValues(camara);
                }
                else
                {
                    _context.Set<Camara>().Update(camara); // Usamos _context.Set<Camara>()
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar cámara");
                return false;
            }
        }


        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.Set<Camara>().FindAsync(id); // Usamos _context.Set<Camara>().FindAsync(id)
                if (entity == null) return false;

                _context.Set<Camara>().Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar cámara");
                return false;
            }
        }
    }
}