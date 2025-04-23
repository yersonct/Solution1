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
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPersonService _personService; // Necesitas el servicio de Person para obtener el nombre

        public UsersController(IUserService userService, IPersonService personService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _personService = personService ?? throw new ArgumentNullException(nameof(personService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            var userDtos = await Task.WhenAll(users.Select(async u =>
            {
                var person = await _personService.GetPersonByIdAsync(u.id_person);
                return new UserDTO
                {
                    Id = u.id,
                    UserName = u.username,
                    PersonId = u.id_person,
                    Password = u.password,
                    PersonName = person?.name
                };
            }));
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var person = await _personService.GetPersonByIdAsync(user.id_person);
            var userDto = new UserDTO
            {
                Id = user.id,
                UserName = user.username,
                PersonId = user.id_person,
                PersonName = person?.name
            };
            return Ok(userDto);
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] UserCreateDTO userCreateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                username = userCreateDTO.UserName,
                password = userCreateDTO.Password,
                id_person = userCreateDTO.PersonId
                // Si agregaste el campo Active en User, podrías inicializarlo aquí:
                // Active = true
            };

            var createdUser = await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.id }, createdUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserCreateDTO userUpdateDTO)
        {
            if (id != userUpdateDTO.id) // Ahora comparamos el id de la ruta con el id del usuario en el DTO
            {
                return BadRequest("El ID del usuario en la ruta no coincide con el ID del usuario en el cuerpo de la petición.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _userService.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.username = userUpdateDTO.UserName;
            existingUser.password = userUpdateDTO.Password;
            existingUser.id_person = userUpdateDTO.PersonId;

            var result = await _userService.UpdateUserAsync(existingUser);
            if (!result)
            {
                return StatusCode(500, "Ocurrió un error al actualizar el usuario.");
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}