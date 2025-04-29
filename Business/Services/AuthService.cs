using BCrypt.Net;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Context;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IloginRepository _userRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IloginRepository userRepository, IPersonRepository personRepository, IConfiguration configuration, ApplicationDbContext context, ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _personRepository = personRepository;
            _configuration = configuration;
            _context = context;
            _logger = logger;
        }

        public async Task<LoginResponse> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null || !VerifyPassword(password, user.password))
            {
                return null;
            }
            var token = GenerateJwtToken(user);
            return new LoginResponse { Token = token, Expiration = DateTime.UtcNow.AddHours(8) };
        }

        public async Task<Dictionary<string, string>> RegisterAsync(RegisterRequest request)
        {
            var errors = new Dictionary<string, string>();

            // 1. Verificar si el nombre de usuario, documento o email ya existen
            // Truncar el nombre de usuario para la verificación
            string truncatedUsernameForCheck = request.Username.Length > 19 ? request.Username.Substring(0, 19) : request.Username;
            var existingUser = await _userRepository.GetUserByUsernameAsync(truncatedUsernameForCheck);
            var personExistsByDocument = await _personRepository.PersonExistsAsync(request.Person.Document, "");
            var personExistsByEmail = await _personRepository.PersonExistsAsync("", request.Person.Email);

            if (existingUser != null)
            {
                errors.Add("Username", "El nombre de usuario ya existe.");
            }

            if (personExistsByDocument)
            {
                errors.Add("Document", "Ya existe una persona registrada con este documento.");
            }

            if (personExistsByEmail)
            {
                errors.Add("Email", "Ya existe una persona registrada con este correo electrónico.");
            }

            if (errors.Any())
            {
                return errors;
            }

            // 2. Crear la entidad Person
            var person = new Person
            {
                name = request.Person.Name,
                lastname = request.Person.LastName,
                document = request.Person.Document,
                phone = request.Person.Phone,
                email = request.Person.Email,
                active = true
            };

            await _personRepository.AddAsync(person);

            // 3. Crear la entidad User
            // Truncar el nombre de usuario si es demasiado largo
            string truncatedUsername = request.Username.Length > 19 ? request.Username.Substring(0, 19) : request.Username;
            var user = new User
            {
                username = truncatedUsername, // Usar el nombre de usuario truncado
                password = HashPassword(request.Password),
                id_person = person.id,
                active = true
            };

            _context.Users.Add(user);
            var result = await _context.SaveChangesAsync() > 0;

            return result ? null : new Dictionary<string, string>() { { "General", "Error al registrar el usuario." } };
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                // Intenta verificar con la configuración predeterminada de BCrypt
                if (BCrypt.Net.BCrypt.Verify(password, hashedPassword))
                {
                    return true;
                }

                // **SOLUCIÓN TEMPORAL (Solo si sabes que tus hashes antiguos usaban SHA1)**
                // Intenta verificar con SHA1 (menos seguro, ¡planifica la migración!)
                // **¡CUIDADO!** La forma de verificar hashes SHA1 con BCrypt.Net puede no ser directa.
                // Es posible que necesites una lógica separada si tus hashes antiguos realmente eran SHA1 sin "salting" de BCrypt.
                // Si tus hashes antiguos fueron creados *por* BCrypt.Net pero con un HashType específico,
                // entonces la verificación con la configuración predeterminada *debería* manejarlos si la librería es reciente.
                // Si aún necesitas intentarlo explícitamente (dependiendo de tu versión de BCrypt.Net):
                try
                {
                    if (BCrypt.Net.BCrypt.Verify(password, hashedPassword, enhancedEntropy: false))
                    {
                        _logger.LogWarning($"Usuario intentó iniciar sesión con un hash de contraseña antiguo (posiblemente SHA1). Usuario: ...");
                        return true;
                    }
                }
                catch (BCrypt.Net.SaltParseException) { /* Ignorar si no es un hash de BCrypt */ }

                // **SOLUCIÓN TEMPORAL (Solo si sabes que tus hashes antiguos usaban SHA256)**
                // Intenta verificar con SHA256 (menos seguro que el predeterminado)
                try
                {
                    if (BCrypt.Net.BCrypt.Verify(password, hashedPassword, enhancedEntropy: false))
                    {
                        _logger.LogWarning($"Usuario intentó iniciar sesión con un hash de contraseña antiguo (posiblemente SHA256). Usuario: ...");
                        return true;
                    }
                }
                catch (BCrypt.Net.SaltParseException) { /* Ignorar si no es un hash de BCrypt */ }

                return false;
            }
            catch (BCrypt.Net.SaltParseException ex)
            {
                _logger.LogError(ex, "Error al verificar la contraseña debido a un problema con el salt.");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al verificar la contraseña.");
                return false;
            }
        }

        private string GenerateJwtToken(User user)
        {
            var secretKey = _configuration.GetSection("JwtSettings:SecretKey").Value;
            var issuer = _configuration.GetSection("JwtSettings:Issuer").Value;
            var audience = _configuration.GetSection("JwtSettings:Audience").Value;
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.username),
                    new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // **FUNCIÓN DE MIGRACIÓN (Ejecutar una sola vez para re-hashear contraseñas)**
        public async Task MigratePasswordsAsync()
        {
            _logger.LogInformation("Iniciando la migración FORZADA de contraseñas...");
            var users = await _userRepository.GetAllUsersAsync();
            int migratedCount = 0;

            foreach (var user in users)
            {
                _logger.LogInformation($"Re-hasheando contraseña para el usuario: {user.username} (ID: {user.id})");
                user.password = HashPassword(user.password); // Fuerza el re-hasheo con la configuración actual
                // Truncar el nombre de usuario antes de actualizarlo
                user.username = user.username.Length > 19 ? user.username.Substring(0, 19) : user.username;
                await _userRepository.UpdateAsync(user);
                migratedCount++;
            }

            _logger.LogInformation($"Migración FORZADA de contraseñas completada. Se re-hashearon {migratedCount} contraseñas.");
        }
    }
}