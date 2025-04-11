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
    public class ClientController : ControllerBase
    {
        private readonly ClientBusiness _clientBusiness;
        private readonly ILogger<ClientController> _logger;

        public ClientController(ClientBusiness clientBusiness, ILogger<ClientController> logger)
        {
            _clientBusiness = clientBusiness;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var clients = await _clientBusiness.GetAllAsync();
                return Ok(clients);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error transitorio al obtener clientes.");
                return StatusCode(500, "Error de conexión a la base de datos.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener clientes.");
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var client = await _clientBusiness.GetByIdAsync(id);
                return Ok(client);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error transitorio al obtener cliente por ID.");
                return StatusCode(500, "Error de conexión a la base de datos.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener cliente por ID.");
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClientCreateDTO dto)
        {
            try
            {
                var created = await _clientBusiness.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error transitorio al crear cliente.");
                return StatusCode(500, "Error de conexión a la base de datos.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear cliente.");
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClientCreateDTO dto)
        {
            try
            {
                var result = await _clientBusiness.UpdateAsync(id, dto);
                return result ? NoContent() : StatusCode(500, "No se pudo actualizar el cliente.");
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
                _logger.LogError(ex, "Error transitorio al actualizar cliente.");
                return StatusCode(500, "Error de conexión a la base de datos.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar cliente.");
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _clientBusiness.DeleteAsync(id);
                return result ? NoContent() : StatusCode(500, "No se pudo eliminar el cliente.");
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error transitorio al eliminar cliente.");
                return StatusCode(500, "Error de conexión a la base de datos.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al eliminar cliente.");
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }
    }
}
