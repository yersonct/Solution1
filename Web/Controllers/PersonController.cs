using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Entity.Model;
using Entity.DTOs; // Asegúrate de tener el namespace de tus DTOs

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonsController(IPersonService personService)
        {
            _personService = personService ?? throw new ArgumentNullException(nameof(personService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonDTO>>> GetAllPersons()
        {
            var persons = await _personService.GetAllPersonsAsync();
            var personDtos = persons.Select(p => new PersonDTO
            {
                Id = p.id,
                Name = p.name,
                LastName = p.lastname,
                Document = p.document,
                Phone = p.phone,
                Email = p.email,
                active = p.active
            }).ToList();
            return Ok(personDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonDTO>> GetPersonById(int id)
        {
            var person = await _personService.GetPersonByIdAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            var personDto = new PersonDTO
            {
                Id = person.id,
                Name = person.name,
                LastName = person.lastname,
                Document = person.document,
                Phone = person.phone,
                Email = person.email,
                active = person.active
            };
            return Ok(personDto);
        }

        [HttpPost]
        public async Task<ActionResult<Person>> CreatePerson([FromBody] PersonDTO createPersonDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var person = new Person
            {
                name = createPersonDTO.Name,
                lastname = createPersonDTO.LastName,
                document = createPersonDTO.Document,
                phone = createPersonDTO.Phone,
                email = createPersonDTO.Email,
                active = true // Ensure new persons are active
            };

            var createdPerson = await _personService.CreatePersonAsync(person);
            return CreatedAtAction(nameof(GetPersonById), new { id = createdPerson.id }, createdPerson);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(int id, [FromBody] PersonDTO updatePersonDTO)
        {
            if (id != updatePersonDTO.Id)
            {
                return BadRequest("El ID de la persona no coincide con el ID de la ruta.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingPerson = await _personService.GetPersonByIdAsync(id);
            if (existingPerson == null)
            {
                return NotFound();
            }

            existingPerson.name = updatePersonDTO.Name;
            existingPerson.lastname = updatePersonDTO.LastName;
            existingPerson.document = updatePersonDTO.Document;
            existingPerson.phone = updatePersonDTO.Phone;
            existingPerson.email = updatePersonDTO.Email;
            existingPerson.active = updatePersonDTO.active; // Allow updating the active status

            var result = await _personService.UpdatePersonAsync(existingPerson);
            if (!result)
            {
                return StatusCode(500, "Ocurrió un error al actualizar la persona.");
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var result = await _personService.DeletePersonAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}