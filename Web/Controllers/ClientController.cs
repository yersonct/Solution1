using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IUserService _userService; // Asegúrate de inyectar IUserService si lo necesitas aquí

        public ClientsController(IClientService clientService, IUserService userService)
        {
            _clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientDTO>>> GetAllClients()
        {
            var clients = await _clientService.GetAllClientsAsync();
            // Proyecta los resultados a ClientDTO incluyendo UserName
            var clientDtos = clients.Select(client => new ClientDTO
            {
                Id = client.id,
                Name = client.name,
                id_user = client.id_user,
                UserName = client.user?.username // Necesitas que la propiedad 'user' esté cargada (eager loading)
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
            // Proyecta el resultado a ClientDTO incluyendo UserName
            var clientDto = new ClientDTO
            {
                Id = client.id,
                Name = client.name,
                id_user = client.id_user,
                UserName = client.user?.username // Necesitas que la propiedad 'user' esté cargada
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
            // Devuelve un DTO en la respuesta incluyendo UserName (si quieres)
            var createdClientDto = new ClientDTO
            {
                Id = createdClient.id,
                Name = createdClient.name,
                id_user = createdClient.id_user
                // Puedes optar por cargar el usuario aquí y también incluir UserName en la respuesta
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