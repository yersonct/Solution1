using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Entity.DTOs;
using Microsoft.Extensions.Logging;
using Entity.Model;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly ILogger<InvoicesController> _logger;

        public InvoicesController(IInvoiceService invoiceService, ILogger<InvoicesController> logger)
        {
            _invoiceService = invoiceService ?? throw new ArgumentNullException(nameof(invoiceService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAllInvoices()
        {
            try
            {
                var invoices = await _invoiceService.GetAllInvoicesAsync();
                var results = invoices.Select(invoice => new
                {
                    id = invoice.id,
                    totalAmount = invoice.totalamount,
                    paymentStatus = invoice.paymentstatus,
                    paymentDate = invoice.paymentdate,
                    vehicleHistoryId = invoice.id_vehiclehistory
                });
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las facturas.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetInvoiceById(int id)
        {
            try
            {
                var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
                if (invoice == null)
                {
                    _logger.LogWarning("Factura no encontrada con ID: {Id}", id);
                    return NotFound();
                }
                var result = new
                {
                    id = invoice.id,
                    totalAmount = invoice.totalamount,
                    paymentStatus = invoice.paymentstatus,
                    paymentDate = invoice.paymentdate,
                    vehicleHistoryId = invoice.id_vehiclehistory
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la factura por ID: {Id}", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<InvoiceDTO>> CreateInvoice([FromBody] InvoiceDTO invoiceDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Modelo de datos no válido al crear una nueva factura.");
                return BadRequest(ModelState);
            }
            try
            {
                var invoice = new Invoice
                {
                    totalamount = invoiceDTO.totalamount,
                    paymentstatus = invoiceDTO.paymentstatus,
                    paymentdate = invoiceDTO.paymentdate,
                    id_vehiclehistory = invoiceDTO.vehiclehistoryid
                };
                var createdInvoice = await _invoiceService.CreateInvoiceAsync(invoice);
                var result = new
                {
                    id = createdInvoice.id,
                    totalAmount = createdInvoice.totalamount,
                    paymentStatus = createdInvoice.paymentstatus,
                    paymentDate = createdInvoice.paymentdate,
                    vehicleHistoryId = createdInvoice.id_vehiclehistory
                };

                return CreatedAtAction(nameof(GetInvoiceById), new { id = createdInvoice.id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear una nueva factura.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvoice(int id, [FromBody] InvoiceDTO invoiceDTO)
        {
            if (id != invoiceDTO.id)
            {
                _logger.LogWarning("Intento de actualizar factura con ID no coincidente. Ruta ID: {RouteId}, DTO ID: {DtoId}", id, invoiceDTO.id);
                return BadRequest("El ID de la factura no coincide con el ID de la ruta.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Modelo de datos no válido al actualizar la factura con ID: {Id}", id);
                return BadRequest(ModelState);
            }

            try
            {
                var existingInvoice = await _invoiceService.GetInvoiceByIdAsync(id);
                if (existingInvoice == null)
                {
                    _logger.LogWarning("Factura no encontrada para actualizar con ID: {Id}", id);
                    return NotFound();
                }
                existingInvoice.totalamount = invoiceDTO.totalamount;
                existingInvoice.paymentstatus = invoiceDTO.paymentstatus;
                existingInvoice.paymentdate = invoiceDTO.paymentdate;
                existingInvoice.id_vehiclehistory = invoiceDTO.vehiclehistoryid;

                var result = await _invoiceService.UpdateInvoiceAsync(existingInvoice);
                if (!result)
                {
                    _logger.LogError("Error al actualizar la factura con ID: {Id}", id);
                    return StatusCode(500, "Error interno del servidor.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la factura con ID: {Id}", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            try
            {
                var result = await _invoiceService.DeleteInvoiceAsync(id);
                if (!result)
                {
                    _logger.LogWarning("Factura no encontrada para eliminar con ID: {Id}", id);
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la factura con ID: {Id}", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }
    }
}

