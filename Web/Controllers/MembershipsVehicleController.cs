using Business;
using Entity.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Utilities.Exceptions;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembershipsVehicleController : ControllerBase
    {
        private readonly MembershipsVehicleBusiness _business;
        private readonly ILogger<MembershipsVehicleController> _logger;

        public MembershipsVehicleController(MembershipsVehicleBusiness business, ILogger<MembershipsVehicleController> logger)
        {
            _business = business;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _business.GetAllAsync();
                return Ok(list);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error transitorio al obtener membresías-vehículos.");
                return StatusCode(500, "Error de conexión a la base de datos.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener membresías-vehículos.");
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var item = await _business.GetByIdAsync(id);
                return Ok(item);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error transitorio al obtener membresía-vehículo.");
                return StatusCode(500, "Error de conexión a la base de datos.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener membresía-vehículo.");
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MemberShipsVehicleCreateDTO dto)
        {
            try
            {
                var created = await _business.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.id }, created);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error transitorio al crear membresía-vehículo.");
                return StatusCode(500, "Error de conexión a la base de datos.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear membresía-vehículo.");
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MemberShipsVehicleCreateDTO dto)
        {
            try
            {
                var result = await _business.UpdateAsync(id, dto);
                return result ? NoContent() : StatusCode(500, "No se pudo actualizar la membresía-vehículo.");
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error transitorio al actualizar membresía-vehículo.");
                return StatusCode(500, "Error de conexión a la base de datos.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar membresía-vehículo.");
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _business.DeleteAsync(id);
                return result ? NoContent() : StatusCode(500, "No se pudo eliminar la membresía-vehículo.");
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error transitorio al eliminar membresía-vehículo.");
                return StatusCode(500, "Error de conexión a la base de datos.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al eliminar membresía-vehículo.");
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }
    }
}
