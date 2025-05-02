using Business.Interfaces;
using Entity.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TuProyecto.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _authService.AuthenticateAsync(request.Username, request.Password);

            if (response == null)
            {
                return Unauthorized(new { message = "Credenciales inválidas" });
            }

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var errors = await _authService.RegisterAsync(request);

            if (errors == null)
            {
                return Ok(new { message = "Registro exitoso" });
            }
            else
            {
                return BadRequest(new { errors = errors }); // Devolver diccionario de errores
            }
        }
    }
}