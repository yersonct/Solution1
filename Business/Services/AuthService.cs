﻿using BCrypt.Net;
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
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IloginRepository _userRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IConfiguration _configuration;
        private readonly IApplicationDbContextWithEntry _context;    
        private readonly ILogger<AuthService> _logger;

        public AuthService(IloginRepository userRepository, IPersonRepository personRepository, IConfiguration configuration, IApplicationDbContextWithEntry context, ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _personRepository = personRepository;
            _configuration = configuration;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<LoginResponse> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null || !VerifyPassword(password, user.Password))
            {
                return null;
            }
            var token = GenerateJwtToken(user);
            return new LoginResponse { Token = token, Expiration = DateTime.UtcNow.AddHours(8) };
        }

        public async Task<Dictionary<string, string>> RegisterAsync(RegisterRequest request)
        {
            var errors = new Dictionary<string, string>();

            if (request.Password.Length > 100)
            {
                errors.Add("Password", "La contraseña es demasiado larga.");
            }

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

            var person = new Person
            {
                Name = request.Person.Name,
                Lastname = request.Person.LastName,
                Document = request.Person.Document,
                Phone = request.Person.Phone,
                Email = request.Person.Email,
                Active = true
            };

            try
            {
                _context.Persons.Add(person);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar la información de la persona durante el registro.");
                return new Dictionary<string, string>() { { "General", "Error al registrar la información de la persona." } };
            }

            string truncatedUsername = request.Username.Length > 19 ? request.Username.Substring(0, 19) : request.Username;
            var user = new User
            {
                Username = truncatedUsername,
                Password = HashPassword(request.Password),
                PersonId = person.Id,
                Active = true
            };

            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar la información del usuario durante el registro.");
                return new Dictionary<string, string>() { { "General", "Error al registrar el usuario." } };
            }
        }


        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string inputPassword, string storedPassword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(storedPassword))
                    return false;

                if (!storedPassword.StartsWith("$2a$") && !storedPassword.StartsWith("$2b$") && !storedPassword.StartsWith("$2y$"))
                {
                    _logger.LogWarning("Comparando contraseña en texto plano. Esto es inseguro y debe corregirse tras la migración.");
                    return inputPassword == storedPassword;
                }
                return BCrypt.Net.BCrypt.Verify(inputPassword, storedPassword);
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
            var secretKey = _configuration.GetSection("Jwt:Key").Value;
            var issuer = _configuration.GetSection("Jwt:Issuer").Value;
            var audience = _configuration.GetSection("Jwt:Audience").Value;
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(3),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task MigratePasswordsAsync()
        {
            _logger.LogInformation("Iniciando la migración FORZADA de contraseñas...");
            var users = await _userRepository.GetAllUsersAsync();
            int migratedCount = 0;

            foreach (var user in users)
            {
                _logger.LogInformation($"Re-hasheando contraseña para el usuario: {user.Username} (ID: {user.Id})");
                user.Password = HashPassword(user.Password);
                user.Username = user.Username.Length > 20 ? user.Username.Substring(0, 19) : user.Username;
                await _userRepository.UpdateAsync(user);
                migratedCount++;
            }

            _logger.LogInformation($"Migración FORZADA de contraseñas completada. Se re-hashearon {migratedCount} contraseñas.");
        }
    }
}

