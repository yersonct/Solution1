// Business/Services/UserService.cs

using Business.Interfaces;
using Business.Validations; // Para tus validaciones de negocio
using Data.Interfaces;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper; // Necesario para AutoMapper
using Entity.DTOs;
using BCrypt.Net; // Añade la referencia a BCrypt.Net-Core (o similar) para hashing

namespace Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPersonService _personService; // Necesitamos este servicio aquí para obtener el nombre de la persona
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository userRepository,
            IPersonService personService, // Inyectamos IPersonService
            ILogger<UserService> logger,
            IMapper mapper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _personService = personService ?? throw new ArgumentNullException(nameof(personService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersWithPersonNameAsync()
        {
            _logger.LogInformation("Obteniendo todos los usuarios para DTOs.");
            var users = await _userRepository.GetAllAsync(); // Obtiene entidades User
            var userDtos = new List<UserDTO>();

            foreach (var user in users)
            {
                var userDto = _mapper.Map<UserDTO>(user); // Mapea User a UserDTO
                var person = await _personService.GetPersonByIdAsync(user.PersonId); // Obtiene la persona
                userDto.PersonName = person?.Name; // Asigna el nombre de la persona
                userDtos.Add(userDto);
            }
            return userDtos;
        }

        public async Task<UserDTO?> GetUserWithPersonNameByIdAsync(int id)
        {
            _logger.LogInformation("Obteniendo usuario con ID {Id} para DTO.", id);
            var user = await _userRepository.GetByIdAsync(id); // Obtiene la entidad User
            if (user == null)
            {
                _logger.LogWarning("Usuario con ID {Id} no encontrado en el repositorio.", id);
                return null;
            }

            var userDto = _mapper.Map<UserDTO>(user); // Mapea User a UserDTO
            var person = await _personService.GetPersonByIdAsync(user.PersonId); // Obtiene la persona
            userDto.PersonName = person?.Name; // Asigna el nombre de la persona
            return userDto;
        }

        public async Task<UserDTO> CreateUserAsync(UserCreateDTO userCreateDto)
        {
            _logger.LogInformation("Creando nuevo usuario desde DTO.");

            // Validaciones de negocio antes de guardar
            var existingUsers = await _userRepository.GetAllAsync();
            LogicValidations.EnsureUserNameIsUnique(existingUsers, userCreateDto.UserName);

            var user = _mapper.Map<User>(userCreateDto); // Mapea DTO de creación a entidad
            user.Password = BCrypt.Net.BCrypt.HashPassword(userCreateDto.Password); // **HASHING DE CONTRASEÑA**
            user.Active = userCreateDto.Active; // Usa el valor de Active del DTO

            var createdUser = await _userRepository.AddAsync(user); // Guarda la entidad
            _logger.LogInformation("Usuario con ID {Id} creado en la base de datos.", createdUser.Id);

            // Mapea la entidad creada (que ya tiene el ID) a UserDTO para devolver
            var userDto = _mapper.Map<UserDTO>(createdUser);
            var person = await _personService.GetPersonByIdAsync(createdUser.PersonId);
            userDto.PersonName = person?.Name; // Rellena el nombre de la persona

            return userDto;
        }

        public async Task<bool> UpdateUserAsync(int id, UserUpdateDTO userUpdateDto)
        {
            _logger.LogInformation("Actualizando usuario con ID {Id} desde DTO.", id);
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                _logger.LogWarning("Intento de actualizar usuario con ID {Id} falló: no encontrado.", id);
                return false;
            }

            // Validaciones de negocio para la actualización
            // Si el nombre de usuario se cambia, asegurarse de que sigue siendo único
            if (existingUser.Username != userUpdateDto.UserName)
            {
                var allUsers = await _userRepository.GetAllAsync();
                LogicValidations.EnsureUserNameIsUnique(allUsers.Where(u => u.Id != id), userUpdateDto.UserName);
            }

            // Mapear los campos actualizables del DTO a la entidad existente
            _mapper.Map(userUpdateDto, existingUser);

            // Solo hashear y actualizar la contraseña si se proporciona una nueva
            if (!string.IsNullOrEmpty(userUpdateDto.Password))
            {
                existingUser.Password = BCrypt.Net.BCrypt.HashPassword(userUpdateDto.Password);
            }

            existingUser.Active = userUpdateDto.Active; // Actualizar el estado activo

            var result = await _userRepository.UpdateAsync(existingUser);
            if (result)
            {
                _logger.LogInformation("Usuario con ID {Id} actualizado exitosamente.", id);
            }
            else
            {
                _logger.LogError("Error al actualizar usuario con ID {Id} en el repositorio.", id);
            }
            return result;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            _logger.LogInformation("Realizando borrado lógico de usuario con ID {Id}.", id);
            var userToDelete = await _userRepository.GetByIdAsync(id);
            if (userToDelete == null)
            {
                _logger.LogWarning("Intento de borrado lógico de usuario con ID {Id} falló: no encontrado.", id);
                return false;
            }

            userToDelete.Active = false; // Borrado lógico: marcar como inactivo
            var result = await _userRepository.UpdateAsync(userToDelete);
            if (result)
            {
                _logger.LogInformation("Usuario con ID {Id} eliminado lógicamente exitosamente.", id);
            }
            else
            {
                _logger.LogError("Error al realizar borrado lógico de usuario con ID {Id} en el repositorio.", id);
            }
            return result;
        }

        // CONSIDERACIONES:
        // - Implementación de LogicValidations: Asegúrate de que LogicValidations lance excepciones claras
        //   que puedan ser capturadas por tu middleware de errores.
        // - Inyección de AutoMapper: Necesitarás configurar AutoMapper en Program.cs/Startup.cs
        //   y crear un perfil de mapeo (MappingProfile) para User, UserDTO, UserCreateDTO, UserUpdateDTO.
    }
}