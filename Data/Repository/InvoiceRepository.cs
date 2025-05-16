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
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly IApplicationDbContext _context; // Cambiado a IApplicationDbContext
        private readonly ILogger<InvoiceRepository> _logger;

        public InvoiceRepository(IApplicationDbContext context, ILogger<InvoiceRepository> logger) // Cambiado a IApplicationDbContext
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Invoice> AddAsync(Invoice entity)
        {
            try
            {
                await _context.Set<Invoice>().AddAsync(entity); // Usar _context.Set<Invoice>()
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar la factura.");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var invoiceToDelete = await _context.Set<Invoice>().FindAsync(id); // Usar _context.Set<Invoice>()
                if (invoiceToDelete != null)
                {
                    _context.Set<Invoice>().Remove(invoiceToDelete); // Usar _context.Set<Invoice>()
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la factura con ID: {InvoiceId}", id);
                return false;
            }
        }

        public async Task<IEnumerable<Invoice>> GetAllAsync()
        {
            try
            {
                return await _context.Set<Invoice>() // Usar _context.Set<Invoice>()
                    .Include(i => i.vehiclehistory) // Asumo que tienes esta relación
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las facturas.");
                return new List<Invoice>();
            }
        }

        public async Task<Invoice?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Invoice>() // Usar _context.Set<Invoice>()
                    .Include(i => i.vehiclehistory) // Asumo que tienes esta relación
                    .FirstOrDefaultAsync(i => i.id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la factura con ID: {InvoiceId}", id);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(Invoice entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Error de concurrencia al actualizar la factura con ID: {InvoiceId}", entity.id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la factura con ID: {InvoiceId}", entity.id);
                return false;
            }
        }
    }
}