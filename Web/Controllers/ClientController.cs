using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Entity.Model;
using Entity.DTOs; // Importa tus DTOs

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientDTO>>> GetAllClients()
        {
            var clients = await _clientService.GetAllClientsAsync();
            // Proyecta los resultados a ClientDTO
            var clientDtos = clients.Select(client => new ClientDTO
            {
                Id = client.id,
                Name = client.name,
                id_user = client.id_user
                // No incluyas BlackListId ni VehicleId aquí, ya que no están directamente en la entidad Client
            }).ToList();
            return Ok(clientDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDTO>> GetClientById(int id)
        {
            var client = await _clientService.GetClientByIdAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            // Proyecta el resultado a ClientDTO
            var clientDto = new ClientDTO
            {
                Id = client.id,
                Name = client.name,
                id_user = client.id_user
                // No incluyas BlackListId ni VehicleId aquí
            };
            return Ok(clientDto);
        }

        [HttpPost]
        public async Task<ActionResult<Client>> CreateClient([FromBody] ClientCreateDTO clientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Mapea desde ClientCreateDTO a la entidad Client
            var client = new Client
            {
                name = clientDto.name,
                id_user = clientDto.id_user,
            };

            var createdClient = await _clientService.CreateClientAsync(client);
            // Devuelve un DTO en la respuesta
            var createdClientDto = new ClientDTO
            {
                Id = createdClient.id,
                Name = createdClient.name,
                id_user = createdClient.id_user
            };
            return CreatedAtAction(nameof(GetClientById), new { id = createdClientDto.Id }, createdClientDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(int id, [FromBody] ClientDTO clientDto)
        {
            if (id != clientDto.Id)
            {
                return BadRequest("El ID del cliente no coincide con el ID de la ruta.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Mapea desde ClientDTO a la entidad Client
            var client = new Client
            {
                id = clientDto.Id,
                name = clientDto.Name,
                id_user = clientDto.id_user,
            };

            var updateSuccessful = await _clientService.UpdateClientAsync(client);
            if (!updateSuccessful)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var result = await _clientService.DeleteClientAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}

