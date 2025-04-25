using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Business.Interfaces; // Asegúrate de ajustar el namespace
using Entity.DTOs; // Asegúrate de ajustar el namespace
[ApiController]
[Route("api/auth")] // Define la ruta base para este controlador
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    // Inyecta la dependencia de IAuthService a través del constructor
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")] // Endpoint específico para el login (api/auth/login)
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // Devuelve errores de validación si el modelo no es válido
        }

        var response = await _authService.AuthenticateAsync(request.Username, request.Password);

        if (response == null)
        {
            return Unauthorized(new { message = "Credenciales inválidas" }); // Devuelve 401 si la autenticación falla
        }

        return Ok(response); // Devuelve 200 OK con el token y la expiración
    }
}
