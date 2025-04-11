using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business;
using Entity.DTOs;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingController : ControllerBase
    {
        private readonly ParkingBusiness _parkingBusiness;
        private readonly ILogger<ParkingController> _logger;

        public ParkingController(ParkingBusiness parkingBusiness, ILogger<ParkingController> logger)
        {
            _parkingBusiness = parkingBusiness ?? throw new ArgumentNullException(nameof(parkingBusiness));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Obtener todos los parkings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParkingDTO>>> GetAll()
        {
            try
            {
                var parkings = await _parkingBusiness.GetAllAsync();

                // Mapear Parking a ParkingDTO
                var parkingDtos = parkings.Select(parking => new ParkingDTO
                {
                    id = parking.id,
                    name = parking.name,
                    location = parking.location,
                    id_camara = parking.id_camara // Asegurarse de mapear id_camara
                }).ToList();

                return Ok(parkingDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los parkings.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        // Obtener un parking por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ParkingDTO>> GetById(int id)
        {
            try
            {
                var parking = await _parkingBusiness.GetByIdAsync(id);
                if (parking == null)
                {
                    return NotFound();
                }

                // Mapear Parking a ParkingDTO
                var parkingDto = new ParkingDTO
                {
                    id = parking.id,
                    name = parking.name,
                    location = parking.location,
                    id_camara = parking.id_camara // Incluir id_camara en el DTO
                };

                return Ok(parkingDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el parking con ID {ParkingId}.", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        // Crear un nuevo parking
        [HttpPost]
        public async Task<ActionResult<ParkingDTO>> Create(ParkingDTO parkingDto)
        {
            try
            {
                if (parkingDto == null)
                {
                    return BadRequest("El objeto parking no puede ser nulo.");
                }

                // Mapear ParkingDTO a Parking
                var parking = new Parking
                {
                    name = parkingDto.name,
                    location = parkingDto.location,
                    id_camara = parkingDto.id_camara,
                    hability = parkingDto.hability // Incluir el campo 'hability'
                };

                // Validación: Verificar que el campo hability no esté vacío
                if (string.IsNullOrEmpty(parking.hability))
                {
                    return BadRequest("El campo 'hability' no puede estar vacío.");
                }

                var createdParking = await _parkingBusiness.CreateAsync(parking);

                // Mapear Parking a ParkingDTO
                var createdParkingDto = new ParkingDTO
                {
                    id = createdParking.id,
                    name = createdParking.name,
                    location = createdParking.location,
                    id_camara = createdParking.id_camara,
                    hability = createdParking.hability // Asegúrate de incluir hability en el DTO
                };

                return CreatedAtAction(nameof(GetById), new { id = createdParkingDto.id }, createdParkingDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el parking.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        // Actualizar un parking existente
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, ParkingDTO parkingDto)
        {
            try
            {
                if (parkingDto == null || id != parkingDto.id)
                {
                    return BadRequest("Los datos del parking no son válidos.");
                }

                // Validación de que el campo 'hability' no esté vacío
                if (string.IsNullOrEmpty(parkingDto.hability))
                {
                    return BadRequest("El campo 'hability' no puede estar vacío.");
                }

                // Mapear ParkingDTO a Parking
                var parking = new Parking
                {
                    id = parkingDto.id,
                    name = parkingDto.name,
                    location = parkingDto.location,
                    id_camara = parkingDto.id_camara,
                    hability = parkingDto.hability // Asegurarse de incluir hability en la entidad
                };

                var success = await _parkingBusiness.UpdateAsync(parking);
                if (!success)
                {
                    return NotFound();
                }

                return NoContent(); // Respuesta 204 No Content
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el parking con ID {ParkingId}.", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        // Eliminar un parking por ID
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var success = await _parkingBusiness.DeleteAsync(id);
                if (!success)
                {
                    return NotFound();
                }

                return NoContent(); // Respuesta 204 No Content
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el parking con ID {ParkingId}.", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }
    }
}
