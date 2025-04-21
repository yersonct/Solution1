using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Entity.Model;
using Entity.DTOs;
using System.Linq;
using Newtonsoft.Json;
using System.Xml;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehiclesController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService ?? throw new ArgumentNullException(nameof(vehicleService));
        }

        [HttpGet]
        public async Task<ActionResult<string>> GetAllVehicles()
        {
            var vehicles = await _vehicleService.GetAllVehiclesAsync();
            var vehicleDtos = vehicles.Select(v => new
            {
                id = v.id,
                plate = v.plate,
                color = v.color,
                id_Client = v.id_client,
                clientName = v.client?.name
            }).ToList();

            var json = JsonConvert.SerializeObject(vehicleDtos, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            return Ok(json);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<string>> GetVehicleById(int id)
        {
            var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            var vehicleDto = new
            {
                id = vehicle.id,
                plate = vehicle.plate,
                color = vehicle.color,
                id_Client = vehicle.id_client,
                clientName = vehicle.client?.name
            };


            var json = JsonConvert.SerializeObject(vehicleDto, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            return Ok(json);
        }

        [HttpPost]
        public async Task<ActionResult<Vehicle>> CreateVehicle([FromBody] VehicleCreateDTO vehicleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehicle = new Vehicle
            {
                plate = vehicleDto.plate,
                color = vehicleDto.color,
                id_client = vehicleDto.id_client,
            };

            var createdVehicle = await _vehicleService.CreateVehicleAsync(vehicle);
            return CreatedAtAction(nameof(GetVehicleById), new { id = createdVehicle.id }, createdVehicle);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody] VehicleDTO vehicleDto)
        {
            if (id != vehicleDto.Id)
            {
                return BadRequest("El ID del vehículo no coincide con el ID de la ruta.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingVehicle = await _vehicleService.GetVehicleByIdAsync(id);
            if (existingVehicle == null)
            {
                return NotFound();
            }

            existingVehicle.plate = vehicleDto.Plate;
            existingVehicle.color = vehicleDto.Color;
            existingVehicle.id_client = vehicleDto.Id_Client;


            var result = await _vehicleService.UpdateVehicleAsync(existingVehicle);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var result = await _vehicleService.DeleteVehicleAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
