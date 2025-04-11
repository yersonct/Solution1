using Dapper;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
    public class InvoiceData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InvoiceData> _logger;

        public InvoiceData(ApplicationDbContext context, ILogger<InvoiceData> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region Métodos con LINQ to Entities

        public async Task<Invoice?> GetLinqByIdAsync(int id)
        {
            try
            {
                return await _context.Invoice.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error (LINQ) al obtener la factura con ID: {InvoiceId}", id);
                return null;
            }
        }

        public async Task<IEnumerable<Invoice>> GetAllLinqAsync()
        {
            try
            {
                return await _context.Invoice.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error (LINQ) al obtener todas las facturas.");
                return new List<Invoice>();
            }
        }

        public async Task<Invoice> CreateLinqAsync(Invoice invoice)
        {
            try
            {
                _context.Invoice.Add(invoice);
                await _context.SaveChangesAsync();
                return invoice;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error (LINQ) al crear una nueva factura.");
                return null;
            }
        }

        public async Task<Invoice> UpdateLinqAsync(Invoice invoice)
        {
            try
            {
                _context.Entry(invoice).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return invoice;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error (LINQ) al actualizar la factura con ID: {InvoiceId}.", invoice.id);
                return null;
            }
        }

        public async Task<bool> DeleteLinqAsync(int id)
        {
            try
            {
                var invoiceToDelete = await _context.Invoice.FindAsync(id);
                if (invoiceToDelete != null)
                {
                    _context.Invoice.Remove(invoiceToDelete);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error (LINQ) al eliminar la factura con ID: {InvoiceId}.", id);
                return false;
            }
        }

        #endregion

        #region Métodos con SQL y Dapper

        public async Task<Invoice?> GetSqlByIdAsync(int id)
        {
            try
            {
                var sql = "SELECT id, totalamount, paymentstatus, paymentdate, id_vehiclehistory FROM invoice WHERE id = @id";
                return await _context.QueryFirstOrDefaultAsync<Invoice>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error (SQL) al obtener la factura con ID: {InvoiceId}", id);
                return null;
            }
        }

        public async Task<IEnumerable<Invoice>> GetAllSqlAsync()
        {
            try
            {
                var sql = "SELECT id, totalamount, paymentstatus, paymentdate, id_vehiclehistory FROM invoice";
                return await _context.QueryAsync<Invoice>(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error (SQL) al obtener todas las facturas.");
                return new List<Invoice>();
            }
        }

        public async Task<Invoice> CreateSqlAsync(Invoice invoice)
        {
            try
            {
                var sql = "INSERT INTO invoice (totalamount, paymentstatus, paymentdate, id_vehiclehistory) VALUES (@totalamount, @paymentstatus, @paymentdate, @idvehiclehistory) RETURNING id";
                var id = await _context.QuerySingleAsync<int>(sql, new
                {
                    totalamount = invoice.totalamount,
                    paymentstatus = invoice.paymentstatus,
                    paymentdate = invoice.paymentdate,
                    idvehiclehistory = invoice.id_vehiclehistory
                });
                invoice.id = id;
                return invoice;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error (SQL) al crear una nueva factura.");
                return null;
            }
        }

        public async Task<bool> UpdateSqlAsync(Invoice invoice)
        {
            try
            {
                var sql = "UPDATE invoice SET totalamount = @totalamount, paymentstatus = @paymentstatus, paymentdate = @paymentdate, id_vehiclehistory = @idvehiclehistory WHERE id = @id";
                var affectedRows = await _context.ExecuteAsync(sql, new
                {
                    id = invoice.id,
                    totalamount = invoice.totalamount,
                    paymentstatus = invoice.paymentstatus,
                    paymentdate = invoice.paymentdate,
                    idvehiclehistory = invoice.id_vehiclehistory
                });
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error (SQL) al actualizar la factura con ID: {InvoiceId}.", invoice.id);
                return false;
            }
        }

        public async Task<bool> DeleteSqlAsync(int id)
        {
            try
            {
                var sql = "DELETE FROM invoice WHERE id = @id";
                var affectedRows = await _context.ExecuteAsync(sql, new { Id = id });
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error (SQL) al eliminar la factura con ID: {InvoiceId}.", id);
                return false;
            }
        }

        #endregion
    }
}