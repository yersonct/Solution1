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
    public class ParkingsController : ControllerBase
    {
        private readonly IParkingService _parkingService;

        public ParkingsController(IParkingService parkingService)
        {
            _parkingService = parkingService ?? throw new ArgumentNullException(nameof(parkingService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Parking>>> GetAllParkings()
        {
            var parkings = await _parkingService.GetAllParkingsAsync();
            return Ok(parkings);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Parking>> GetParkingById(int id)
        {
            var parking = await _parkingService.GetParkingByIdAsync(id);
            if (parking == null)
            {
                return NotFound();
            }
            return Ok(parking);
        }

        [HttpPost]
        public async Task<ActionResult<Parking>> CreateParking([FromBody] Parking parking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdParking = await _parkingService.CreateParkingAsync(parking);
            return CreatedAtAction(nameof(GetParkingById), new { id = createdParking.id }, createdParking);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateParking(int id, [FromBody] Parking parking)
        {
            if (id != parking.id)
            {
                return BadRequest("El ID del parking no coincide con el ID de la ruta.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _parkingService.UpdateParkingAsync(parking);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParking(int id)
        {
            var result = await _parkingService.DeleteParkingAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}