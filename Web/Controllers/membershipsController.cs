using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Entity.Model;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembershipsController : ControllerBase
    {
        private readonly IMembershipsService _membershipsService;

        public MembershipsController(IMembershipsService membershipsService)
        {
            _membershipsService = membershipsService ?? throw new ArgumentNullException(nameof(membershipsService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberShips>>> GetAllMemberships()
        {
            var memberships = await _membershipsService.GetAllMembershipsAsync();
            return Ok(memberships);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MemberShips>> GetMembershipById(int id)
        {
            var membership = await _membershipsService.GetMembershipByIdAsync(id);
            if (membership == null)
            {
                return NotFound();
            }
            return Ok(membership);
        }

        [HttpPost]
        public async Task<ActionResult<MemberShips>> CreateMembership([FromBody] MemberShips membership)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdMembership = await _membershipsService.CreateMembershipAsync(membership);
            return CreatedAtAction(nameof(GetMembershipById), new { id = createdMembership.id }, createdMembership);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMembership(int id, [FromBody] MemberShips membership)
        {
            if (id != membership.id)
            {
                return BadRequest("El ID de la membresía no coincide con el ID de la ruta.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _membershipsService.UpdateMembershipAsync(membership);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMembership(int id)
        {
            var result = await _membershipsService.DeleteMembershipAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}