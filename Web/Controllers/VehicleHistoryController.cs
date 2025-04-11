using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business;
using Entity.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleHistoryController : ControllerBase
    {
        private readonly VehicleHistoryBusiness _vehicleHistoryBusiness;
        private readonly ILogger<VehicleHistoryController> _logger;

        public VehicleHistoryController(VehicleHistoryBusiness vehicleHistoryBusiness, ILogger<VehicleHistoryController> logger)
        {
            _vehicleHistoryBusiness = vehicleHistoryBusiness ?? throw new ArgumentNullException(nameof(vehicleHistoryBusiness));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Entity.DTOs.VehicleHistoryDTO>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var vehicleHistories = await _vehicleHistoryBusiness.GetAllAsync();
                return Ok(vehicleHistories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los registros de VehicleHistory en el controlador.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor.");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Entity.DTOs.VehicleHistoryDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var vehicleHistory = await _vehicleHistoryBusiness.GetByIdAsync(id);
                if (vehicleHistory == null)
                {
                    return NotFound();
                }
                return Ok(vehicleHistory);
            }
            catch (Utilities.Exceptions.EntityNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el registro de VehicleHistory con ID {Id} en el controlador.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor.");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Entity.DTOs.VehicleHistoryDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] VehicleHistoryCreateDTO dto)
        {
            try
            {
                if (dto == null || !ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdVehicleHistory = await _vehicleHistoryBusiness.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = createdVehicleHistory.id }, createdVehicleHistory);
            }
            catch (Utilities.Exceptions.ValidationException ex)
            {
                ModelState.AddModelError(ex.PropertyName ?? "", ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo registro de VehicleHistory en el controlador.", dto);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor.");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] VehicleHistoryCreateDTO dto)
        {
            try
            {
                if (dto == null || !ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updated = await _vehicleHistoryBusiness.UpdateAsync(id, dto);
                if (!updated)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Utilities.Exceptions.EntityNotFoundException)
            {
                return NotFound();
            }
            catch (Utilities.Exceptions.ValidationException ex)
            {
                ModelState.AddModelError(ex.PropertyName ?? "", ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el registro de VehicleHistory con ID {Id} en el controlador.", id, dto);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor.");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _vehicleHistoryBusiness.DeleteAsync(id);
                if (!deleted)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Utilities.Exceptions.EntityNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el registro de VehicleHistory con ID {Id} en el controlador.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor.");
            }
        }
    }
}