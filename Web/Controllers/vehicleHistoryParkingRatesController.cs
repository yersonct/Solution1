// API/Controllers/VehicleHistoryParkingRatesController.cs
using Business;
using Entity.DTOs;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleHistoryParkingRatesController : ControllerBase
    {
        private readonly VehicleHistoryParkingRatesBusiness _business;
        private readonly ILogger<VehicleHistoryParkingRatesController> _logger;

        public VehicleHistoryParkingRatesController(VehicleHistoryParkingRatesBusiness business, ILogger<VehicleHistoryParkingRatesController> logger)
        {
            _business = business ?? throw new ArgumentNullException(nameof(business));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleHistoryParkingRatesDTO>>> GetAll()
        {
            try
            {
                var vhpr = await _business.GetAllAsync();
                var dtos = vhpr.Select(v => new VehicleHistoryParkingRatesDTO
                {
                    Id = v.id,
                    VehicleHistoryId = v.id_vehiclehistory,
                    RatesId = v.id_rates,
                    ParkingId = v.id_parking,
                    HoursUsed = v.hourused,
                    SubTotal = v.subtotal
                });
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los registros de VehicleHistoryParkingRates en el controlador.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleHistoryParkingRatesDTO>> GetById(int id)
        {
            try
            {
                var vhpr = await _business.GetByIdAsync(id);
                if (vhpr == null)
                {
                    return NotFound();
                }
                var dto = new VehicleHistoryParkingRatesDTO
                {
                    Id = vhpr.id,
                    VehicleHistoryId = vhpr.id_vehiclehistory,
                    RatesId = vhpr.id_rates,
                    ParkingId = vhpr.id_parking,
                    HoursUsed = vhpr.hourused,
                    SubTotal = vhpr.subtotal
                };
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener VehicleHistoryParkingRates con ID {Id} en el controlador.", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<VehicleHistoryParkingRatesDTO>> Create([FromBody] VehicleHistoryParkingRatesDTO createDto)
        {
            try
            {
                if (createDto == null || !ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var vhprToCreate = new VehicleHistoryParkingRates
                {
                    id_vehiclehistory = createDto.VehicleHistoryId,
                    id_rates = createDto.RatesId,
                    id_parking = createDto.ParkingId,
                    hourused = createDto.HoursUsed,
                    subtotal = createDto.SubTotal
                };

                var createdVhpr = await _business.CreateAsync(vhprToCreate);

                var dto = new VehicleHistoryParkingRatesDTO
                {
                    Id = createdVhpr.id,
                    VehicleHistoryId = createdVhpr.id_vehiclehistory,
                    RatesId = createdVhpr.id_rates,
                    ParkingId = createdVhpr.id_parking,
                    HoursUsed = createdVhpr.hourused,
                    SubTotal = createdVhpr.subtotal
                };

                return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo registro de VehicleHistoryParkingRates en el controlador.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VehicleHistoryParkingRatesDTO updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.Id || !ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingVhpr = await _business.GetByIdAsync(id);
                if (existingVhpr == null)
                {
                    return NotFound();
                }

                existingVhpr.id_vehiclehistory = updateDto.VehicleHistoryId;
                existingVhpr.id_rates = updateDto.RatesId;
                existingVhpr.id_parking = updateDto.ParkingId;
                existingVhpr.hourused = updateDto.HoursUsed;
                existingVhpr.subtotal = updateDto.SubTotal;

                var updated = await _business.UpdateAsync(existingVhpr);
                if (updated)
                {
                    return NoContent();
                }
                else
                {
                    return StatusCode(500, "Error al actualizar el registro.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el registro de VehicleHistoryParkingRates con ID {Id} en el controlador.", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _business.DeleteAsync(id);
                if (deleted)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el registro de VehicleHistoryParkingRates con ID {Id} en el controlador.", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

      

       

    }
}