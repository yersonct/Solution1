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
    public class FormsController : ControllerBase
    {
        private readonly IFormService _formService;

        public FormsController(IFormService formService)
        {
            _formService = formService ?? throw new ArgumentNullException(nameof(formService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FormDTO>>> GetAllForms()
        {
            var forms = await _formService.GetAllFormsAsync();
            var formDtos = forms.Select(f => new FormDTO
            {
                id = f.id,
                name = f.name,
                url = f.url
            }).ToList();
            return Ok(formDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FormDTO>> GetFormById(int id)
        {
            var form = await _formService.GetFormByIdAsync(id);
            if (form == null)
            {
                return NotFound();
            }

            var formDto = new FormDTO
            {
                id = form.id,
                name = form.name,
                url = form.url
            };
            return Ok(formDto);
        }

        [HttpPost]
        public async Task<ActionResult<Forms>> CreateForm([FromBody] FormDTO formDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var form = new Forms
            {
                name = formDto.name,
                url = formDto.url
            };

            var createdForm = await _formService.CreateFormAsync(form);
            return CreatedAtAction(nameof(GetFormById), new { id = createdForm.id }, createdForm);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateForm(int id, [FromBody] FormDTO formDto)
        {
            if (id != formDto.id)
            {
                return BadRequest("El ID del formulario no coincide con el ID de la ruta.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingForm = await _formService.GetFormByIdAsync(id);
            if (existingForm == null)
            {
                return NotFound();
            }

            existingForm.name = formDto.name;
            existingForm.url = formDto.url;


            var result = await _formService.UpdateFormAsync(existingForm);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteForm(int id)
        {
            var result = await _formService.DeleteFormAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}