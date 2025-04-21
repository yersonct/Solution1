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
    public class TypeRatesController : ControllerBase
    {
        private readonly ITypeRatesService _typeRatesService;

        public TypeRatesController(ITypeRatesService typeRatesService)
        {
            _typeRatesService = typeRatesService ?? throw new ArgumentNullException(nameof(typeRatesService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeRates>>> GetAllTypeRates()
        {
            var typeRates = await _typeRatesService.GetAllTypeRatesAsync();
            return Ok(typeRates);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TypeRates>> GetTypeRatesById(int id)
        {
            var typeRates = await _typeRatesService.GetTypeRatesByIdAsync(id);
            if (typeRates == null)
            {
                return NotFound();
            }
            return Ok(typeRates);
        }

        [HttpPost]
        public async Task<ActionResult<TypeRates>> CreateTypeRates([FromBody] TypeRates typeRates)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdTypeRates = await _typeRatesService.CreateTypeRatesAsync(typeRates);
            return CreatedAtAction(nameof(GetTypeRatesById), new { id = createdTypeRates.Id }, createdTypeRates);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTypeRates(int id, [FromBody] TypeRates typeRates)
        {
            if (id != typeRates.Id)
            {
                return BadRequest("El ID del tipo de tarifa no coincide con el ID de la ruta.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _typeRatesService.UpdateTypeRatesAsync(typeRates);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTypeRates(int id)
        {
            var result = await _typeRatesService.DeleteTypeRatesAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}