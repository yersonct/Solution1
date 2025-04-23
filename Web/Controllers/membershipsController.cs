using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces; // Asegúrate de que esta interfaz exista
using Entity.Model;
using Entity.DTOs; // Asegúrate de que este namespace exista
using System.Linq;

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
        public async Task<ActionResult<IEnumerable<MembershipDTO>>> GetAllMemberships()
        {
            var memberships = await _membershipsService.GetAllMembershipsAsync();
            var membershipDtos = memberships.Select(m => new MembershipDTO
            {
                id = m.id,
                membershiptype = m.membershiptype,
                startdate = m.startdate,
                enddate = m.enddate,
                active = m.active
            }).ToList();
            return Ok(membershipDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MembershipDTO>> GetMembershipById(int id)
        {
            var membership = await _membershipsService.GetMembershipByIdAsync(id);
            if (membership == null)
            {
                return NotFound();
            }

            var membershipDto = new MembershipDTO
            {
                id = membership.id,
                membershiptype = membership.membershiptype,
                startdate = membership.startdate,
                enddate = membership.enddate,
                active = membership.active
            };
            return Ok(membershipDto);
        }

        [HttpPost]
        public async Task<ActionResult<MemberShips>> CreateMembership([FromBody] MembershipDTO membershipDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var membership = new MemberShips
            {
                membershiptype = membershipDto.membershiptype,
                startdate = membershipDto.startdate,
                enddate = membershipDto.enddate,
                active = membershipDto.active
            };

            var createdMembership = await _membershipsService.CreateMembershipAsync(membership);
            return CreatedAtAction(nameof(GetMembershipById), new { id = createdMembership.id }, createdMembership);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMembership(int id, [FromBody] MembershipDTO membershipDto)
        {
            if (id != membershipDto.id)
            {
                return BadRequest("El ID de la membresía no coincide con el ID de la ruta.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingMembership = await _membershipsService.GetMembershipByIdAsync(id);
            if (existingMembership == null)
            {
                return NotFound();
            }

            existingMembership.membershiptype = membershipDto.membershiptype;
            existingMembership.startdate = membershipDto.startdate;
            existingMembership.enddate = membershipDto.enddate;
            existingMembership.active = membershipDto.active;


            var result = await _membershipsService.UpdateMembershipAsync(existingMembership);
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