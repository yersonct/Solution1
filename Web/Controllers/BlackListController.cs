using Business;
using Business.Interfaces;
using Entity.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Exceptions;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlackListController : ControllerBase
    {
        private readonly IBlackListService _service;

        public BlackListController(IBlackListService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        //[HttpGet("with-client")]
        //public async Task<IActionResult> GetAllWithClient()
        //{
        //    var result = await _service.GetAllWithClientAsync();
        //    return Ok(result);
        //}

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        //[HttpGet("with-client/{id}")]
        //public async Task<IActionResult> GetByIdWithClient(int id)
        //{
        //    var result = await _service.GetByIdWithClientAsync(id);
        //    if (result == null) return NotFound();
        //    return Ok(result);
        //}

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BlackListDTO dto)
        {
            try
            {
                await _service.CreateAsync(dto);
                return Ok();
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", detail = ex.Message });
            }
        }


        [HttpPut]
        public async Task<IActionResult> Update(BlackListDTO dto)
        {
            await _service.UpdateAsync(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}
