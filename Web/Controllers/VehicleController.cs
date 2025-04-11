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
    public class VehicleController : ControllerBase
    {
        private readonly VehicleBusiness _vehicleBusiness;
        private readonly ILogger<VehicleController> _logger;

        public VehicleController(VehicleBusiness vehicleBusiness, ILogger<VehicleController> logger)
        {
            _vehicleBusiness = vehicleBusiness;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var vehicles = await _vehicleBusiness.GetAllAsync();
                return Ok(vehicles);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error transitorio al obtener vehículos.");
                return StatusCode(500, "Error de conexión a la base de datos.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener vehículos.");
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var vehicle = await _vehicleBusiness.GetByIdAsync(id);
                return Ok(vehicle);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error transitorio al obtener vehículo por ID.");
                return StatusCode(500, "Error de conexión a la base de datos.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener vehículo por ID.");
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VehicleCreateDTO dto)
        {
            try
            {
                var created = await _vehicleBusiness.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error transitorio al crear vehículo.");
                return StatusCode(500, "Error de conexión a la base de datos.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear vehículo.");
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VehicleCreateDTO dto)
        {
            try
            {
                var result = await _vehicleBusiness.UpdateAsync(id, dto);
                return result ? NoContent() : StatusCode(500, "No se pudo actualizar el vehículo.");
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
                _logger.LogError(ex, "Error transitorio al actualizar vehículo.");
                return StatusCode(500, "Error de conexión a la base de datos.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar vehículo.");
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _vehicleBusiness.DeleteAsync(id);
                return result ? NoContent() : StatusCode(500, "No se pudo eliminar el vehículo.");
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error transitorio al eliminar vehículo.");
                return StatusCode(500, "Error de conexión a la base de datos.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al eliminar vehículo.");
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }
    }
}
