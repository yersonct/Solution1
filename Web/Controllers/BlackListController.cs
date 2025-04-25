using Business.Interfaces;
using Entity.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Utilities.Exceptions;

namespace configuracion.Controllers
{
    [ApiController]
    [Route("api/blacklist")]
    public class BlackListController : ControllerBase
    {
        private readonly IBlackListService _blackListService;

        public BlackListController(IBlackListService blackListService)
        {
            _blackListService = blackListService ?? throw new ArgumentNullException(nameof(blackListService));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BlackListDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _blackListService.GetAllAsync();
            return Ok(result); // Ya proyectado en el servicio con clientName y filtrado por active
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BlackListDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _blackListService.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result); // Ya proyectado en el servicio con clientName y filtrado por active
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Create([FromBody] BlackListDTO dto)
        {
            try
            {
                await _blackListService.CreateAsync(dto);
                return Ok();
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "Error interno del servidor", detail = ex.Message });
            }
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update([FromBody] BlackListDTO dto)
        {
            try
            {
                await _blackListService.UpdateAsync(dto);
                return Ok();
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _blackListService.DeleteAsync(id);
                return Ok();
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
        }
    }
}