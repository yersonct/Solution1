using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Entity.DTOs;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Business.Services;
using Microsoft.Extensions.Logging; // Agregado para el logger

namespace configuracion.Controllers
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
                var result = membershipsVehicles.Select(mv => new MembershipsVehicleDTO
                {
                    id = mv.id, // ✅ Asigna el ID
                    VehiclePlate = mv.vehicle?.plate,
                    MembershipsNmae = mv.memberships?.membershiptype,
                    active = mv.active
                }).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetAll");
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

                var result = new MembershipsVehicleDTO // Proyecta con el formato deseado
                {
                    VehiclePlate = createdMembershipVehicle.vehicle?.plate, // Get Vehicle plate
                    MembershipsNmae = createdMembershipVehicle.memberships?.membershiptype, //Get Memberships name
                    active = createdMembershipVehicle.active
                };

                return CreatedAtAction(nameof(GetById), new { id = createdMembershipVehicle.id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en Post"); // Registra el error
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

                var result = new MembershipsVehicleDTO // Proyecta con el formato deseado
                {
                    VehiclePlate = membershipsVehicle.vehicle?.plate, // Get Vehicle plate
                    MembershipsNmae = membershipsVehicle.memberships?.membershiptype, //Get Memberships name
                    active = membershipsVehicle.active
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

                var result = new MembershipsVehicleDTO // Proyecta con el formato deseado
                {
                    VehiclePlate = existingMembershipVehicle.vehicle?.plate, // Get Vehicle plate
                    MembershipsNmae = existingMembershipVehicle.memberships?.membershiptype, //Get Memberships name
                    active = existingMembershipVehicle.active
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