using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Business.Interfaces;
using Entity.DTOs;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CamaraController : ControllerBase
    {
        private readonly ICamaraService _camaraService;
        private readonly ILogger<CamaraController> _logger;

        public CamaraController(ICamaraService camaraService, ILogger<CamaraController> logger)
        {
            _camaraService = camaraService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CamaraDTO>>> GetAll()
        {
            var result = await _camaraService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CamaraDTO>> GetById(int id)
        {
            var camara = await _camaraService.GetByIdAsync(id);
            if (camara == null)
                return NotFound();

            return Ok(camara);
        }

        [HttpPost]
        public async Task<ActionResult<CamaraDTO>> Create([FromBody] CamaraDTO dto)
        {
            if (dto == null)
                return BadRequest("Datos inválidos");

            var created = await _camaraService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CamaraDTO>> Update(int id, [FromBody] CamaraDTO dto)
        {
            if (dto == null || id != dto.id)
                return BadRequest("Datos inconsistentes");

            var updated = await _camaraService.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _camaraService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
