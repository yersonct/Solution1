using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Entity.Model;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService ?? throw new ArgumentNullException(nameof(invoiceService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetAllInvoices()
        {
            var invoices = await _invoiceService.GetAllInvoicesAsync();
            return Ok(invoices);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoiceById(int id)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            return Ok(invoice);
        }

        [HttpPost]
        public async Task<ActionResult<Invoice>> CreateInvoice([FromBody] Invoice invoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdInvoice = await _invoiceService.CreateInvoiceAsync(invoice);
            return CreatedAtAction(nameof(GetInvoiceById), new { id = createdInvoice.id }, createdInvoice);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvoice(int id, [FromBody] Invoice invoice)
        {
            if (id != invoice.id)
            {
                return BadRequest("El ID de la factura no coincide con el ID de la ruta.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _invoiceService.UpdateInvoiceAsync(invoice);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            var result = await _invoiceService.DeleteInvoiceAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}