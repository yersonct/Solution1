using Business;
using Entity.DTOs;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Exceptions;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class MemberShipsController : ControllerBase
    {
        private readonly MembershipsBusiness _MemberShipsBusiness;
        private readonly ILogger<MemberShipsController> _logger;

        public MemberShipsController(MembershipsBusiness MemberShipsBusiness, ILogger<MemberShipsController> logger)
        {
            _MemberShipsBusiness = MemberShipsBusiness;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MembershipDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllMemberShipss()
        {
            try
            {
                var MemberShipss = await _MemberShipsBusiness.GetAllMembershipsAsync();
                return Ok(MemberShipss);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los MemberShipsas");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MembershipDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetMemberShipsById(int id)
        {
            try
            {
                var MemberShips = await _MemberShipsBusiness.GetMembershipByIdAsync(id);
                return Ok(MemberShips);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para usuario con ID: {MemberShipsId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Usuario no encontrado con ID: {MemberShipsId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario con ID: {MemberShipsId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(MembershipDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateMemberShipsAsync([FromBody] MembershipDTO MemberShipsDTO)
        {
            try
            {
                var createdMemberShips = await _MemberShipsBusiness.CreateMembershipAsync(MemberShipsDTO);
                return CreatedAtAction(nameof(GetMemberShipsById), new { id = createdMemberShips.id }, createdMemberShips);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear el MemberShipsas");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear el MemberShipsas");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MembershipDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateMemberShipsAsync(int id, [FromBody] MembershipDTO MemberShipsDTO)
        {
            if (id != MemberShipsDTO.id)
            {
                return BadRequest(new { message = "El ID de la ruta no coincide con el ID del objeto." });
            }
            try
            {


                var updatedMemberShips = await _MemberShipsBusiness.UpdateMembershipAsync(MemberShipsDTO);
                return Ok(updatedMemberShips);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar el MemberShipsas con ID: {MemberShipsId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "MemberShipsas no encontrado con ID: {MemberShipsId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar el usuario con ID: {MemberShipsId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteMemberShipsAsync(int id)
        {
            try
            {
                var deleted = await _MemberShipsBusiness.DeleteMembershipAsync(id);
                if (!deleted)
                    return NotFound(new { message = "MemberShipsas no encontrado o ya eliminado" });

                return Ok(new { message = "MemberShipsas eliminado exitosamente" });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar el MemberShipsas con ID: {UserId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
