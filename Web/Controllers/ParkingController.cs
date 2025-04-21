using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Entity.Model;
using Entity.DTOs;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParkingsController : ControllerBase
    {
        private readonly IParkingService _parkingService;
        private readonly ILogger<ParkingsController> _logger;

        public ParkingsController(IParkingService parkingService, ILogger<ParkingsController> logger)
        {
            _parkingService = parkingService ?? throw new ArgumentNullException(nameof(parkingService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAllParkings()
        {
            try
            {
                var parkings = await _parkingService.GetAllParkingsAsync();
                var result = parkings.Select(p => new
                {
                    id = p.id,
                    name = p.name,
                    location = p.location,
                    hability = p.hability,
                    id_camara = p.id_camara
                });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los parkings.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetParkingById(int id)
        {
            try
            {
                var parking = await _parkingService.GetParkingByIdAsync(id);
                if (parking == null)
                {
                    _logger.LogWarning("Parking no encontrado con ID: {Id}", id);
                    return NotFound();
                }
                var result = new
                {
                    id = parking.id,
                    name = parking.name,
                    location = parking.location,
                    hability = parking.hability,
                    id_camara = parking.id_camara
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el parking con ID: {Id}", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Parking>> CreateParking([FromBody] ParkingDTO parkingDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Modelo de datos no válido al crear un nuevo parking.");
                return BadRequest(ModelState);
            }
            try
            {
                var parking = new Parking
                {
                    name = parkingDTO.name,
                    location = parkingDTO.location,
                    hability = parkingDTO.hability,
                    id_camara = parkingDTO.id_camara
                };
                var createdParking = await _parkingService.CreateParkingAsync(parking);
                var result = new
                {
                    id = createdParking.id,
                    name = createdParking.name,
                    location = createdParking.location,
                    hability = createdParking.hability,
                    id_camara = createdParking.id_camara
                };
                return CreatedAtAction(nameof(GetParkingById), new { id = createdParking.id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo parking.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateParking(int id, [FromBody] ParkingDTO parkingDTO)
        {
            if (id != parkingDTO.id)
            {
                _logger.LogWarning("Intento de actualizar parking con ID no coincidente. Ruta ID: {RouteId}, DTO ID: {DtoId}", id, parkingDTO.id);
                return BadRequest("El ID del parking no coincide con el ID de la ruta.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Modelo de datos no válido al actualizar el parking con ID: {Id}", id);
                return BadRequest(ModelState);
            }
            try
            {
                var existingParking = await _parkingService.GetParkingByIdAsync(id);
                if (existingParking == null)
                {
                    _logger.LogWarning("Parking no encontrado para actualizar con ID: {Id}", id);
                    return NotFound();
                }
                existingParking.name = parkingDTO.name;
                existingParking.location = parkingDTO.location;
                existingParking.hability = parkingDTO.hability;
                existingParking.id_camara = parkingDTO.id_camara;

                var result = await _parkingService.UpdateParkingAsync(existingParking);
                if (!result)
                {
                    _logger.LogError("Error al actualizar el parking con ID: {Id}", id);
                    return StatusCode(500, "Error interno del servidor.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el parking con ID: {Id}", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParking(int id)
        {
            try
            {
                var result = await _parkingService.DeleteParkingAsync(id);
                if (!result)
                {
                    _logger.LogWarning("Parking no encontrado para eliminar con ID: {Id}", id);
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el parking con ID: {Id}", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }
    }
}

