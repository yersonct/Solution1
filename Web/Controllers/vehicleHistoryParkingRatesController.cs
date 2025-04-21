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
    public class VehicleHistoryParkingRatesController : ControllerBase
    {
        private readonly IVehicleHistoryParkingRatesService _vhprService;

        public VehicleHistoryParkingRatesController(IVehicleHistoryParkingRatesService vhprService)
        {
            _vhprService = vhprService ?? throw new ArgumentNullException(nameof(vhprService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleHistoryParkingRates>>> GetAllVehicleHistoryParkingRates()
        {
            var result = await _vhprService.GetAllVehicleHistoryParkingRatesAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleHistoryParkingRates>> GetVehicleHistoryParkingRatesById(int id)
        {
            var result = await _vhprService.GetVehicleHistoryParkingRatesByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<VehicleHistoryParkingRates>> CreateVehicleHistoryParkingRates([FromBody] VehicleHistoryParkingRates vhpr)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _vhprService.CreateVehicleHistoryParkingRatesAsync(vhpr);
            return CreatedAtAction(nameof(GetVehicleHistoryParkingRatesById), new { id = created.id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicleHistoryParkingRates(int id, [FromBody] VehicleHistoryParkingRates vhpr)
        {
            if (id != vhpr.id)
            {
                return BadRequest("El ID de VehicleHistoryParkingRates no coincide con el ID de la ruta.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _vhprService.UpdateVehicleHistoryParkingRatesAsync(vhpr);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleHistoryParkingRates(int id)
        {
            var result = await _vhprService.DeleteVehicleHistoryParkingRatesAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}