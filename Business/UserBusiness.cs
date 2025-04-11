using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    public class UserBusiness
    {
        private readonly UserData _userData;
        private readonly ILogger<UserBusiness> _logger;

        public UserBusiness(UserData userData, ILogger<UserBusiness> logger)
        {
            _userData = userData ?? throw new ArgumentNullException(nameof(userData));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userData.GetAllAsync();
                return MapToDtoList(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los usuarios.");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de usuarios.", ex);
            }
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            if (id <= 0)
                throw new ValidationException("id", "El ID del usuario debe ser mayor que cero.");

            try
            {
                var user = await _userData.GetByIdAsync(id);
                if (user == null)
                    throw new EntityNotFoundException("User", id);

                return MapToDTO(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario con ID: {UserId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el usuario con ID {id}.", ex);
            }
        }

        public async Task<UserDTO> CreateUserAsync(UserCreateDTO userCreateDTO)
        {
            try
            {
                ValidateUserCreate(userCreateDTO);

                var user = new User
                {
                    username = userCreateDTO.UserName,
                    password = userCreateDTO.Password,
                    id_person = userCreateDTO.PersonId
                };

                var createdUser = await _userData.CreateAsyncSQL(user);
                return MapToDTO(createdUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo usuario: {Username}", userCreateDTO?.UserName ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el usuario.", ex);
            }
        }

        public async Task<bool> UpdateUserAsync(UserDTO userDTO)
        {
            ValidateUser(userDTO);

            try
            {
                var existingUser = await _userData.GetByIdAsync(userDTO.Id);
                if (existingUser == null)
                    throw new EntityNotFoundException("User", userDTO.Id);

                existingUser.username = userDTO.UserName;
                existingUser.id_person = userDTO.PersonId;

                return await _userData.UpdateAsyncSQL(existingUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el usuario con ID: {UserId}", userDTO.Id);
                throw new ExternalServiceException("Base de datos", "Error al actualizar el usuario.", ex);
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                var existingUser = await _userData.GetByIdAsync(id);
                if (existingUser == null)
                    throw new EntityNotFoundException("User", id);

                return await _userData.DeleteAsyncSQL(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el usuario con ID: {UserId}", id);
                throw new ExternalServiceException("Base de datos", "Error al eliminar el usuario.", ex);
            }
        }

        private void ValidateUser(UserDTO userDTO)
        {
            if (userDTO == null)
                throw new ValidationException("El objeto usuario no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(userDTO.UserName))
                throw new ValidationException("Username", "El nombre de usuario es obligatorio.");
        }

        private void ValidateUserCreate(UserCreateDTO dto)
        {
            if (dto == null)
                throw new ValidationException("El objeto usuario no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(dto.UserName))
                throw new ValidationException("Username", "El nombre de usuario es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new ValidationException("Password", "La contraseña es obligatoria.");
        }

        private UserDTO MapToDTO(User user)
        {
            return new UserDTO
            {
                Id = user.id,
                UserName = user.username,
                PersonId = user.id_person,
                PersonName = user.person?.name + " " + user.person?.lastname
            };
        }

        private IEnumerable<UserDTO> MapToDtoList(IEnumerable<User> users)
        {
            foreach (var user in users)
                yield return MapToDTO(user);
        }
    }
}
