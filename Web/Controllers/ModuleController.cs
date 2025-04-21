using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Entity.Model;
using Entity.DTOs; // Asegúrate de tener el namespace de tus DTOs
using System.Linq;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModulesController : ControllerBase
    {
        private readonly IModuleService _moduleService;

        public ModulesController(IModuleService moduleService)
        {
            _moduleService = moduleService ?? throw new ArgumentNullException(nameof(moduleService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModuleDTO>>> GetAllModules()
        {
            var modules = await _moduleService.GetAllModulesAsync();
            var moduleDtos = modules.Select(m => new ModuleDTO
            {
                id = m.id,
                name = m.name,
            }).ToList();
            return Ok(moduleDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ModuleDTO>> GetModuleById(int id)
        {
            var module = await _moduleService.GetModuleByIdAsync(id);
            if (module == null)
            {
                return NotFound();
            }

            var moduleDto = new ModuleDTO
            {
                id = module.id,
                name = module.name,
            };
            return Ok(moduleDto);
        }

        [HttpPost]
        public async Task<ActionResult<Modules>> CreateModule([FromBody] ModuleDTO moduleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var module = new Modules
            {
                name = moduleDto.name,
            };

            var createdModule = await _moduleService.CreateModuleAsync(module);
            return CreatedAtAction(nameof(GetModuleById), new { id = createdModule.id }, createdModule);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateModule(int id, [FromBody] ModuleDTO moduleDto)
        {
            if (id != moduleDto.id)
            {
                return BadRequest("El ID del módulo no coincide con el ID de la ruta.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingModule = await _moduleService.GetModuleByIdAsync(id);
            if (existingModule == null)
            {
                return NotFound();
            }

            existingModule.name = moduleDto.name;


            var result = await _moduleService.UpdateModuleAsync(existingModule);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModule(int id)
        {
            var result = await _moduleService.DeleteModuleAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
