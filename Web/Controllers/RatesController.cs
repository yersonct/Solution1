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
    public class RatesController : ControllerBase
    {
        private readonly IRatesService _ratesService;

        public RatesController(IRatesService ratesService)
        {
            _ratesService = ratesService ?? throw new ArgumentNullException(nameof(ratesService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rates>>> GetAllRates()
        {
            var rates = await _ratesService.GetAllRatesAsync();
            return Ok(rates);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Rates>> GetRatesById(int id)
        {
            var rate = await _ratesService.GetRatesByIdAsync(id);
            if (rate == null)
            {
                return NotFound();
            }
            return Ok(rate);
        }

        [HttpPost]
        public async Task<ActionResult<Rates>> CreateRates([FromBody] Rates rates)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdRates = await _ratesService.CreateRatesAsync(rates);
            return CreatedAtAction(nameof(GetRatesById), new { id = createdRates.id }, createdRates);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRates(int id, [FromBody] Rates rates)
        {
            if (id != rates.id)
            {
                return BadRequest("El ID de la tarifa no coincide con el ID de la ruta.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _ratesService.UpdateRatesAsync(rates);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRates(int id)
        {
            var result = await _ratesService.DeleteRatesAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}