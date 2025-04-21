using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Entity.DTOs;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormModulesController : ControllerBase
    {
        private readonly IFormModuleService _formModuleService;

        public FormModulesController(IFormModuleService formModuleService)
        {
            _formModuleService = formModuleService ?? throw new ArgumentNullException(nameof(formModuleService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FormModuleDTO>>> GetAllFormModules()
        {
            var formModules = await _formModuleService.GetAllFormModulesAsync();
            return Ok(formModules);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FormModuleDTO>> GetFormModuleById(int id)
        {
            var formModule = await _formModuleService.GetFormModuleByIdAsync(id);
            if (formModule == null)
            {
                return NotFound();
            }
            return Ok(formModule);
        }

        [HttpPost]
        public async Task<ActionResult<FormModuleDTO>> CreateFormModule([FromBody] FormModuleCreateDTO formModule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdFormModule = await _formModuleService.CreateFormModuleAsync(formModule);
            return CreatedAtAction(nameof(GetFormModuleById), new { id = createdFormModule.Id }, createdFormModule);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFormModule(int id, [FromBody] FormModuleCreateDTO formModule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _formModuleService.UpdateFormModuleAsync(id, formModule);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFormModule(int id)
        {
            var result = await _formModuleService.DeleteFormModuleAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}