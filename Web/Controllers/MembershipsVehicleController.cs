using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Entity.DTOs;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Business.Services;
using Microsoft.Extensions.Logging; // Agregado para el logger

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembershipsVehiclesController : ControllerBase
    {
        private readonly MembershipsVehicleService _membershipsVehicleService;
        private readonly ILogger<MembershipsVehiclesController> _logger; // Agregado logger

        public MembershipsVehiclesController(MembershipsVehicleService membershipsVehicleService, ILogger<MembershipsVehiclesController> logger)
        {
            _membershipsVehicleService = membershipsVehicleService;
            _logger = logger; // Inyecta el logger
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var membershipsVehicles = await _membershipsVehicleService.GetAllMembershipsVehiclesAsync();
                var result = membershipsVehicles.Select(mv => new
                {
                    id = mv.id,
                    vehicleId = mv.vehicle?.id, // Agregado manejo de nulos
                    vehiclePlate = mv.vehicle?.plate, // Agregado manejo de nulos
                    membershipsId = mv.memberships?.id, // Agregado manejo de nulos
                    membershipsType = mv.memberships?.membershiptype, // Agregado manejo de nulos
                    membershipsStartDate = mv.memberships?.startdate, // Agregado manejo de nulos
                    membershipsEndDate = mv.memberships?.enddate, // Agregado manejo de nulos
                    membershipsActive = mv.memberships?.active // Agregado manejo de nulos
                }).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetAll"); // Registra el error
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MemberShipsVehicleCreateDTO createDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var membershipsVehicle = new MembershipsVehicle
            {
                id_memberships = createDTO.membershipsid,
                id_vehicle = createDTO.vehicleid
            };

            try
            {
                var createdMembershipVehicle = await _membershipsVehicleService.CreateMembershipsVehicleAsync(membershipsVehicle);

                var result = new // Proyecta con el formato deseado
                {
                    id = createdMembershipVehicle.id,
                    vehicleId = createdMembershipVehicle.vehicle?.id,  // Agregado manejo de nulos
                    vehiclePlate = createdMembershipVehicle.vehicle?.plate, // Agregado manejo de nulos
                    membershipsId = createdMembershipVehicle.memberships?.id, // Agregado manejo de nulos
                    membershipsType = createdMembershipVehicle.memberships?.membershiptype, // Agregado manejo de nulos
                    membershipsStartDate = createdMembershipVehicle.memberships?.startdate, // Agregado manejo de nulos
                    membershipsEndDate = createdMembershipVehicle.memberships?.enddate, // Agregado manejo de nulos
                    membershipsActive = createdMembershipVehicle.memberships?.active  // Agregado manejo de nulos
                };

                return CreatedAtAction(nameof(GetById), new { id = createdMembershipVehicle.id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en Post");  // Registra el error
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var membershipsVehicle = await _membershipsVehicleService.GetMembershipsVehicleByIdAsync(id);
                if (membershipsVehicle == null)
                {
                    return NotFound();
                }

                var result = new  // Proyecta con el formato deseado
                {
                    id = membershipsVehicle.id,
                    vehicleId = membershipsVehicle.vehicle?.id, // Agregado manejo de nulos
                    vehiclePlate = membershipsVehicle.vehicle?.plate, // Agregado manejo de nulos
                    membershipsId = membershipsVehicle.memberships?.id, // Agregado manejo de nulos
                    membershipsType = membershipsVehicle.memberships?.membershiptype, // Agregado manejo de nulos
                    membershipsStartDate = membershipsVehicle.memberships?.startdate, // Agregado manejo de nulos
                    membershipsEndDate = membershipsVehicle.memberships?.enddate, // Agregado manejo de nulos
                    membershipsActive = membershipsVehicle.memberships?.active  // Agregado manejo de nulos
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetById"); // Registra error
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] MembershipsVehicleDTO updateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingMembershipVehicle = await _membershipsVehicleService.GetMembershipsVehicleByIdAsync(id);
                if (existingMembershipVehicle == null)
                {
                    return NotFound();
                }

                existingMembershipVehicle.id_memberships = updateDTO.id_memberships;
                existingMembershipVehicle.id_vehicle = updateDTO.id_vehicle;

                await _membershipsVehicleService.UpdateMembershipsVehicleAsync(existingMembershipVehicle);

                var result = new // Proyecta con el formato deseado
                {
                    id = existingMembershipVehicle.id,
                    vehicleId = existingMembershipVehicle.vehicle?.id, // Agregado manejo de nulos
                    vehiclePlate = existingMembershipVehicle.vehicle?.plate, // Agregado manejo de nulos
                    membershipsId = existingMembershipVehicle.memberships?.id, // Agregado manejo de nulos
                    membershipsType = existingMembershipVehicle.memberships?.membershiptype, // Agregado manejo de nulos
                    membershipsStartDate = existingMembershipVehicle.memberships?.startdate, // Agregado manejo de nulos
                    membershipsEndDate = existingMembershipVehicle.memberships?.enddate, // Agregado manejo de nulos
                    membershipsActive = existingMembershipVehicle.memberships?.active  // Agregado manejo de nulos
                };
                return Ok(result);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error in Put"); // Registra error
                return StatusCode(500, $"Database update error: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error in Put"); // Registra error
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _membershipsVehicleService.DeleteMembershipsVehicleAsync(id);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Delete"); // Registra error
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

