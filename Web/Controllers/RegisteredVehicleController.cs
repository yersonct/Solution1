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
    public class RegisteredVehicleController : ControllerBase
    {
        private readonly RegisteredVehicleBusiness _registeredVehicleBusiness;
        private readonly ILogger<RegisteredVehicleController> _logger;

        public RegisteredVehicleController(RegisteredVehicleBusiness registeredVehicleBusiness, ILogger<RegisteredVehicleController> logger)
        {
            _registeredVehicleBusiness = registeredVehicleBusiness;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _registeredVehicleBusiness.GetAllAsync();
                return Ok(list);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error transitorio al obtener registros de vehículos.");
                return StatusCode(500, "Error de conexión a la base de datos.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener registros de vehículos.");
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _registeredVehicleBusiness.GetByIdAsync(id);
                return Ok(result);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error transitorio al obtener registro de vehículo por ID.");
                return StatusCode(500, "Error de conexión a la base de datos.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener registro de vehículo por ID.");
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RegisteredVehicleCreateDTO dto)
        {
            try
            {
                var created = await _registeredVehicleBusiness.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.id }, created);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error transitorio al crear registro de vehículo.");
                return StatusCode(500, "Error de conexión a la base de datos.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear registro de vehículo.");
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RegisteredVehicleCreateDTO dto)
        {
            try
            {
                var result = await _registeredVehicleBusiness.UpdateAsync(id, dto);
                return result ? NoContent() : StatusCode(500, "No se pudo actualizar el registro de vehículo.");
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
                _logger.LogError(ex, "Error transitorio al actualizar registro de vehículo.");
                return StatusCode(500, "Error de conexión a la base de datos.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar registro de vehículo.");
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _registeredVehicleBusiness.DeleteAsync(id);
                return result ? NoContent() : StatusCode(500, "No se pudo eliminar el registro de vehículo.");
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error transitorio al eliminar registro de vehículo.");
                return StatusCode(500, "Error de conexión a la base de datos.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al eliminar registro de vehículo.");
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }
    }
}
