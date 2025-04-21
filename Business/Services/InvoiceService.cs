using Business.Interfaces;
using Data.Interfaces;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ILogger<InvoiceService> _logger;

        public InvoiceService(IInvoiceRepository invoiceRepository, ILogger<InvoiceService> logger)
        {
            _invoiceRepository = invoiceRepository ?? throw new ArgumentNullException(nameof(invoiceRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Invoice>> GetAllInvoicesAsync()
        {
            return await _invoiceRepository.GetAllAsync();
        }

        public async Task<Invoice?> GetInvoiceByIdAsync(int id)
        {
            return await _invoiceRepository.GetByIdAsync(id);
        }

        public async Task<Invoice> CreateInvoiceAsync(Invoice invoice)
        {
            // Aquí podrías agregar lógica de negocio antes de crear la factura
            return await _invoiceRepository.AddAsync(invoice);
        }

        public async Task<bool> UpdateInvoiceAsync(Invoice invoice)
        {
            // Aquí podrías agregar lógica de negocio antes de actualizar la factura
            return await _invoiceRepository.UpdateAsync(invoice);
        }

        public async Task<bool> DeleteInvoiceAsync(int id)
        {
            // Aquí podrías agregar lógica de negocio antes de eliminar la factura
            return await _invoiceRepository.DeleteAsync(id);
        }
    }
}
