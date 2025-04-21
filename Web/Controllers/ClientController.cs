using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Entity.Model;
using Business;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientBusiness;

        public ClientsController(IClientService clientBusiness)
        {
            _clientBusiness = clientBusiness ?? throw new ArgumentNullException(nameof(clientBusiness));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetAllClients()
        {
            var clients = await _clientBusiness.GetAllClientsAsync();
            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClientById(int id)
        {
            var client = await _clientBusiness.GetClientByIdAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        [HttpPost]
        public async Task<ActionResult<Client>> CreateClient([FromBody] Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdClient = await _clientBusiness.CreateClientAsync(client);
            return CreatedAtAction(nameof(GetClientById), new { id = createdClient.id }, createdClient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(int id, [FromBody] Client client)
        {
            if (id != client.id)
            {
                return BadRequest("El ID del cliente no coincide con el ID de la ruta.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _clientBusiness.UpdateClientAsync(client);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var result = await _clientBusiness.DeleteClientAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}