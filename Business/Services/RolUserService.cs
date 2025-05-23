using AutoMapper; // Necesario para AutoMapper
using Business.Interfaces;
using Data.Interfaces;
using Entity.DTOs;    // Para RolUserDTO y RolUserCreateDTO
using Entity.Model;   // Para RolUser
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class RolUserService : IRolUserService
    {
        private readonly IRolUserRepository _rolUserRepository;
        private readonly IMapper _mapper; // Inyectamos IMapper
        private readonly ILogger<RolUserService> _logger;

        public RolUserService(IRolUserRepository rolUserRepository,
                              IMapper mapper, // Añadir IMapper al constructor
                              ILogger<RolUserService> logger)
        {
            _rolUserRepository = rolUserRepository ?? throw new ArgumentNullException(nameof(rolUserRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper)); // Inicializar IMapper
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Método de interfaz debería devolver DTOs
        public async Task<IEnumerable<RolUserDTO>> GetAllRolUsersAsync()
        {
            // Obtiene entidades del repositorio
            var rolUsers = await _rolUserRepository.GetAllAsync();
            // Mapea las entidades a DTOs para el retorno
            return _mapper.Map<IEnumerable<RolUserDTO>>(rolUsers);
        }

        // Método de interfaz debería devolver DTOs
        public async Task<RolUserDTO?> GetRolUserByIdAsync(int id)
        {
            // Obtiene la entidad del repositorio
            var rolUser = await _rolUserRepository.GetByIdAsync(id);
            // Mapea la entidad a un DTO
            return _mapper.Map<RolUserDTO>(rolUser);
        }

        // Método de interfaz debería recibir DTO de creación y devolver DTO de lectura
        public async Task<RolUserDTO> CreateRolUserAsync(RolUserCreateDTO rolUserDto) // Cambiado para recibir DTO
        {
            // Mapear el DTO de creación a la entidad del modelo
            var rolUserEntity = _mapper.Map<RolUser>(rolUserDto);

            rolUserEntity.Active = true; // Establecer active a true al crear (lógica de negocio)

            // El repositorio agrega la entidad
            var createdEntity = await _rolUserRepository.AddAsync(rolUserEntity);

            // Mapear la entidad creada de vuelta a un DTO para el retorno
            // Asegúrate de que el repositorio cargue las propiedades de navegación (Rol, User) si RolUserDTO las necesita
            return _mapper.Map<RolUserDTO>(createdEntity);
        }

        // Método de interfaz debería recibir DTO de actualización y devolver bool
        public async Task<bool> UpdateRolUserAsync(int id, RolUserCreateDTO rolUserDto) // Cambiado para recibir ID y DTO
        {
            var existingRolUser = await _rolUserRepository.GetByIdAsync(id);
            if (existingRolUser == null)
            {
                _logger.LogWarning($"RolUser con ID {id} no encontrado para actualización.");
                return false; // No se encontró para actualizar
            }

            // Mapear los campos del DTO de actualización a la entidad existente
            // Esto actualiza las propiedades de existingRolUser con los valores de rolUserDto
            _mapper.Map(rolUserDto, existingRolUser);

            // El repositorio actualiza la entidad
            return await _rolUserRepository.UpdateAsync(existingRolUser);
        }

        public async Task<bool> DeleteRolUserAsync(int id)
        {
            var rolUserToDelete = await _rolUserRepository.GetByIdAsync(id);
            if (rolUserToDelete == null)
            {
                _logger.LogWarning($"RolUser con ID {id} no encontrado para eliminación lógica.");
                return false;
            }
            rolUserToDelete.Active = false; // Borrado lógico (actualiza la entidad)
            return await _rolUserRepository.UpdateAsync(rolUserToDelete);
        }
    }
}