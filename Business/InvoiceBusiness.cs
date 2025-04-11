using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions; // Asegúrate de tener esta librería o crea tus propias excepciones

namespace Business
{
    public class InvoiceBusiness
    {
        private readonly InvoiceData _invoiceData;
        private readonly ILogger<InvoiceBusiness> _logger;

        public InvoiceBusiness(InvoiceData invoiceData, ILogger<InvoiceBusiness> logger)
        {
            _invoiceData = invoiceData ?? throw new ArgumentNullException(nameof(invoiceData));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<InvoiceDTO>> GetAllInvoicesLinqAsync()
        {
            try
            {
                var invoices = await _invoiceData.GetAllLinqAsync();
                return MapToDtoList(invoices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las facturas (LINQ).");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de facturas (LINQ).", ex);
            }
        }

        public async Task<InvoiceDTO> GetInvoiceLinqAsync(int id)
        {
            if (id <= 0)
            {
                throw new ValidationException("id", "El ID de la factura debe ser mayor que cero.");
            }

            try
            {
                var invoice = await _invoiceData.GetLinqByIdAsync(id);
                if (invoice == null)
                {
                    throw new EntityNotFoundException("Invoice", id);
                }
                return MapToDTO(invoice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la factura (LINQ) con ID: {InvoiceId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la factura (LINQ) con ID {id}.", ex);
            }
        }

        public async Task<InvoiceDTO> CreateInvoiceLinqAsync(InvoiceDTO createDTO)
        {
            try
            {
                ValidateInvoice(createDTO);
                var invoice = MapToEntity(createDTO);
                var createdInvoice = await _invoiceData.CreateLinqAsync(invoice);
                return MapToDTO(createdInvoice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear una nueva factura (LINQ). Total: {TotalAmount}", createDTO?.totalamount);
                throw new ExternalServiceException("Base de datos", "Error al crear la factura (LINQ).", ex);
            }
        }

        public async Task<InvoiceDTO> UpdateInvoiceLinqAsync(int id, InvoiceDTO updateDTO)
        {
            if (id <= 0 || updateDTO?.id != id)
            {
                throw new ValidationException("id", "El ID proporcionado no coincide con el ID de la factura a actualizar.");
            }

            try
            {
                ValidateInvoice(updateDTO);
                var existingInvoice = await _invoiceData.GetLinqByIdAsync(id);
                if (existingInvoice == null)
                {
                    throw new EntityNotFoundException("Invoice", id);
                }

                // Mapear los valores del DTO a la entidad EXISTENTE (rastreada)
                existingInvoice.totalamount = updateDTO.totalamount;
                existingInvoice.paymentstatus = updateDTO.paymentstatus;
                existingInvoice.paymentdate = updateDTO.paymentdate;
                existingInvoice.id_vehiclehistory = updateDTO.vehiclehistoryid;
                // No necesitas cambiar la propiedad 'id' ya que es la clave

                var updatedInvoice = await _invoiceData.UpdateLinqAsync(existingInvoice); // Pasa la ENTIDAD RASTREADA
                return MapToDTO(updatedInvoice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la factura (LINQ) con ID: {InvoiceId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al actualizar la factura (LINQ) con ID {id}.", ex);
            }
        }

        public async Task<bool> DeleteInvoiceLinqAsync(int id)
        {
            if (id <= 0)
            {
                throw new ValidationException("id", "El ID de la factura a eliminar debe ser mayor que cero.");
            }

            try
            {
                var existingInvoice = await _invoiceData.GetLinqByIdAsync(id);
                if (existingInvoice == null)
                {
                    throw new EntityNotFoundException("Invoice", id);
                }
                return await _invoiceData.DeleteLinqAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la factura (LINQ) con ID: {InvoiceId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al eliminar la factura (LINQ) con ID {id}.", ex);
            }
        }

        // Repite la estructura anterior para los métodos que utilizan SQL (GetAllInvoicesSqlAsync, GetInvoiceSqlAsync, etc.)
        // llamando a los métodos correspondientes de _invoiceData que terminan en "SqlAsync".

        public async Task<IEnumerable<InvoiceDTO>> GetAllInvoicesSqlAsync()
        {
            try
            {
                var invoices = await _invoiceData.GetAllSqlAsync();
                return MapToDtoList(invoices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las facturas (SQL).");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de facturas (SQL).", ex);
            }
        }

        public async Task<InvoiceDTO> GetInvoiceSqlAsync(int id)
        {
            if (id <= 0)
            {
                throw new ValidationException("id", "El ID de la factura debe ser mayor que cero.");
            }

            try
            {
                var invoice = await _invoiceData.GetSqlByIdAsync(id);
                if (invoice == null)
                {
                    throw new EntityNotFoundException("Invoice", id);
                }
                return MapToDTO(invoice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la factura (SQL) con ID: {InvoiceId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la factura (SQL) con ID {id}.", ex);
            }
        }

        public async Task<InvoiceDTO> CreateInvoiceSqlAsync(InvoiceDTO createDTO)
        {
            try
            {
                ValidateInvoice(createDTO);
                var invoice = MapToEntity(createDTO);
                var createdInvoice = await _invoiceData.CreateSqlAsync(invoice);
                return MapToDTO(createdInvoice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear una nueva factura (SQL). Total: {TotalAmount}", createDTO?.totalamount);
                throw new ExternalServiceException("Base de datos", "Error al crear la factura (SQL).", ex);
            }
        }

        // Asumo que tienes un método UpdateSqlAsync en tu Business Layer si tienes el correspondiente en Data
        // Puedes implementarlo siguiendo la lógica de UpdateInvoiceLinqAsync pero llamando a _invoiceData.UpdateSqlAsync

        public async Task<bool> DeleteInvoiceSqlAsync(int id)
        {
            if (id <= 0)
            {
                throw new ValidationException("id", "El ID de la factura a eliminar debe ser mayor que cero.");
            }

            try
            {
                var existingInvoice = await _invoiceData.GetSqlByIdAsync(id);
                if (existingInvoice == null)
                {
                    throw new EntityNotFoundException("Invoice", id);
                }
                return await _invoiceData.DeleteSqlAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la factura (SQL) con ID: {InvoiceId}.", id);
                throw new ExternalServiceException("Base de datos", $"Error al eliminar la factura (SQL) con ID {id}.", ex);
            }
        }

        private void ValidateInvoice(InvoiceDTO invoiceDTO)
        {
            if (invoiceDTO == null)
            {
                throw new ValidationException("El objeto factura no puede ser nulo.");
            }
            if (invoiceDTO.totalamount < 0)
            {
                throw new ValidationException("TotalAmount", "El monto total no puede ser negativo.");
            }
            // Añade más validaciones según tus reglas de negocio
        }

        private InvoiceDTO MapToDTO(Invoice invoice)
        {
            return new InvoiceDTO
            {
                id = invoice.id,
                totalamount = invoice.totalamount,
                paymentstatus = invoice.paymentstatus,
                paymentdate = invoice.paymentdate,
                vehiclehistoryid = invoice.id_vehiclehistory
            };
        }

        private Invoice MapToEntity(InvoiceDTO invoiceDTO)
        {
            return new Invoice
            {
                id = invoiceDTO.id,
                totalamount = invoiceDTO.totalamount,
                paymentstatus = invoiceDTO.paymentstatus,
                paymentdate = invoiceDTO.paymentdate,
                id_vehiclehistory = invoiceDTO.vehiclehistoryid
            };
        }

        private IEnumerable<InvoiceDTO> MapToDtoList(IEnumerable<Invoice> invoices)
        {
            return invoices.Select(MapToDTO).ToList();
        }
    }
}