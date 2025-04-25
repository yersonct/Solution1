using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Business.Interfaces;
using Entity.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace configuracion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CamaraController : ControllerBase
    {
        private readonly ICamaraService _camaraService;
        private readonly ILogger<CamaraController> _logger;

        public CamaraController(ICamaraService camaraService, ILogger<CamaraController> logger)
        {
            _camaraService = camaraService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAll()
        {
            try
            {
                var result = await _camaraService.GetAllAsync();
                var response = result.Select(c => new
                {
                    id = c.id,
                    name = c.name,
                    nightvisioninfrared = c.nightvisioninfrared,
                    highresolution = c.highresolution,
                    infraredlighting = c.infraredlighting,
                    optimizedangleofvision = c.optimizedangleofvision,
                    highshutterspeed = c.highshutterspeed
                });
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las cámaras.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetById(int id)
        {
            try
            {
                var camara = await _camaraService.GetByIdAsync(id);
                if (camara == null)
                {
                    _logger.LogWarning("Cámara no encontrada con ID: {Id}", id);
                    return NotFound();
                }
                var response = new
                {
                    id = camara.id,
                    name = camara.name,
                    nightvisioninfrared = camara.nightvisioninfrared,
                    highresolution = camara.highresolution,
                    infraredlighting = camara.infraredlighting,
                    optimizedangleofvision = camara.optimizedangleofvision,
                    highshutterspeed = camara.highshutterspeed
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la cámara con ID: {Id}", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CamaraDTO>> Create([FromBody] CamaraDTO dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("Datos de cámara inválidos para la creación.");
                return BadRequest("Datos inválidos");
            }

            try
            {
                var created = await _camaraService.CreateAsync(dto);
                var response = new CamaraDTO
                {
                    id = created.id,
                    name = created.name,
                    nightvisioninfrared = created.nightvisioninfrared,
                    highresolution = created.highresolution,
                    infraredlighting = created.infraredlighting,
                    optimizedangleofvision = created.optimizedangleofvision,
                    highshutterspeed = created.highshutterspeed
                };
                return CreatedAtAction(nameof(GetById), new { id = response.id }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear una nueva cámara.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<object>> Update(int id, [FromBody] CamaraDTO dto)
        {
            if (dto == null || id != dto.id)
            {
                _logger.LogWarning("Datos de cámara inconsistentes para la actualización. ID de ruta: {RouteId}, ID de DTO: {DtoId}", id, dto.id);
                return BadRequest("Datos inconsistentes");
            }

            try
            {
                var updated = await _camaraService.UpdateAsync(id, dto);
                if (updated == null)
                {
                    _logger.LogWarning("Cámara no encontrada para actualizar con ID: {Id}", id);
                    return NotFound();
                }
                var response = new
                {
                    id = updated.id,
                    name = updated.name,
                    nightvisioninfrared = updated.nightvisioninfrared,
                    highresolution = updated.highresolution,
                    infraredlighting = updated.infraredlighting,
                    optimizedangleofvision = updated.optimizedangleofvision,
                    highshutterspeed = updated.highshutterspeed
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la cámara con ID: {Id}", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _camaraService.DeleteAsync(id);
                if (!deleted)
                {
                    _logger.LogWarning("Cámara no encontrada para eliminar con ID: {Id}", id);
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la cámara con ID: {Id}", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }
    }
}

