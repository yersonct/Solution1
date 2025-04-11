using Microsoft.AspNetCore.Mvc;
using Business;
using Entity.DTOs;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly InvoiceBusiness _invoiceBusiness; // Asumiendo que InvoiceBusiness usa LINQ internamente ahora
        private readonly ILogger<InvoicesController> _logger;

        public InvoicesController(InvoiceBusiness invoiceBusiness, ILogger<InvoicesController> logger)
        {
            _invoiceBusiness = invoiceBusiness ?? throw new ArgumentNullException(nameof(invoiceBusiness));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInvoices()
        {
            try
            {
                var invoicesDTO = await _invoiceBusiness.GetAllInvoicesLinqAsync(); // O un método similar sin el sufijo "Linq"
                return Ok(invoicesDTO);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener todas las facturas de la base de datos.");
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener todas las facturas.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoice(int id)
        {
            try
            {
                var invoiceDTO = await _invoiceBusiness.GetInvoiceLinqAsync(id); // O un método similar sin el sufijo "Linq"
                return Ok(invoiceDTO);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { Message = ex.Message, Property = ex.PropertyName });
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener la factura con ID: {InvoiceId} de la base de datos.", id);
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener la factura con ID: {InvoiceId}.", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] InvoiceDTO createDTO)
        {
            try
            {
                var createdInvoiceDTO = await _invoiceBusiness.CreateInvoiceLinqAsync(createDTO); // O un método similar sin el sufijo "Linq"
                return CreatedAtAction(nameof(GetInvoice), new { id = createdInvoiceDTO.id }, createdInvoiceDTO);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { Message = ex.Message, Property = ex.PropertyName });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear la factura en la base de datos. Total: {TotalAmount}", createDTO?.totalamount);
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear la factura. Total: {TotalAmount}", createDTO?.totalamount);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvoice(int id, [FromBody] InvoiceDTO updateDTO)
        {
            try
            {
                var updatedInvoiceDTO = await _invoiceBusiness.UpdateInvoiceLinqAsync(id, updateDTO);
                return Ok(updatedInvoiceDTO);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { Message = ex.Message, Property = ex.PropertyName });
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar la factura con ID: {InvoiceId} en la base de datos.", id);
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar la factura con ID: {InvoiceId}.", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            try
            {
                await _invoiceBusiness.DeleteInvoiceLinqAsync(id); // O un método similar sin el sufijo "Linq"
                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { Message = ex.Message, Property = ex.PropertyName });
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar la factura con ID: {InvoiceId} de la base de datos.", id);
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al eliminar la factura con ID: {InvoiceId}.", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }
    }
}