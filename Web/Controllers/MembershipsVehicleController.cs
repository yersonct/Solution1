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
    public class MembershipsVehiclesController : ControllerBase
    {
        private readonly IMembershipsVehicleService _membershipsVehicleService;

        public MembershipsVehiclesController(IMembershipsVehicleService membershipsVehicleService)
        {
            _membershipsVehicleService = membershipsVehicleService ?? throw new ArgumentNullException(nameof(membershipsVehicleService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MembershipsVehicle>>> GetAllMembershipsVehicles()
        {
            var membershipsVehicles = await _membershipsVehicleService.GetAllMembershipsVehiclesAsync();
            return Ok(membershipsVehicles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MembershipsVehicle>> GetMembershipsVehicleById(int id)
        {
            var membershipsVehicle = await _membershipsVehicleService.GetMembershipsVehicleByIdAsync(id);
            if (membershipsVehicle == null)
            {
                return NotFound();
            }
            return Ok(membershipsVehicle);
        }

        [HttpPost]
        public async Task<ActionResult<MembershipsVehicle>> CreateMembershipsVehicle([FromBody] MembershipsVehicle membershipsVehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdMembershipsVehicle = await _membershipsVehicleService.CreateMembershipsVehicleAsync(membershipsVehicle);
            return CreatedAtAction(nameof(GetMembershipsVehicleById), new { id = createdMembershipsVehicle.id }, createdMembershipsVehicle);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMembershipsVehicle(int id, [FromBody] MembershipsVehicle membershipsVehicle)
        {
            if (id != membershipsVehicle.id)
            {
                return BadRequest("El ID de la relación no coincide con el ID de la ruta.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _membershipsVehicleService.UpdateMembershipsVehicleAsync(membershipsVehicle);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMembershipsVehicle(int id)
        {
            var result = await _membershipsVehicleService.DeleteMembershipsVehicleAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}