using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration; // Para leer la configuración
using Data.Repositories;
using Entity.DTOs;
using Business.Interfaces;
using Org.BouncyCastle.Crypto.Generators;
using Entity.Model; // Asegúrate de ajustar el namespace

namespace TuProyecto.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository; // Usa tu repositorio de usuario
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<LoginResponse> AuthenticateAsync(string username, string password)
        {
            // 1. Autenticar al usuario (usando el repositorio)
            var user = await _userRepository.GetUserByUsernameAsync(username); // Asegúrate de crear este método
            if (user == null || !VerifyPassword(password, user.password)) // Implementa VerifyPassword
            {
                return null; // O lanza una excepción AuthenticationException
            }

            // 2. Generar el Token JWT
            var token = GenerateJwtToken(user);
            return new LoginResponse { Token = token, Expiration = DateTime.UtcNow.AddHours(8) }; //Añadi la expiracion
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            // Implementa la lógica para verificar la contraseña hasheada
            // ¡No compares contraseñas en texto plano!
            // Usa una función como BCrypt.Verify o PasswordHasher.Verify
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }



        private string GenerateJwtToken(User user) // Asegúrate de usar tu clase de usuario
        {
            // Lee la configuración del token desde appsettings.json
            var secretKey = _configuration.GetSection("JwtSettings:SecretKey").Value;
            var issuer = _configuration.GetSection("JwtSettings:Issuer").Value;
            var audience = _configuration.GetSection("JwtSettings:Audience").Value;
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.username),
                    new Claim(ClaimTypes.NameIdentifier, user.id.ToString()), // Asegúrate de que tu usuario tenga un Id
                    // Puedes agregar más claims (roles, permisos, etc.)
                }),
                Expires = DateTime.UtcNow.AddHours(8), // Configura la expiración del token
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
