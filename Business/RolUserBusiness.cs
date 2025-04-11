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
    public class RolUserBusiness
    {
        private readonly RolUserData _rolUserData;
        private readonly ILogger<RolUserBusiness> _logger;

        public RolUserBusiness(RolUserData rolUserData, ILogger<RolUserBusiness> logger)
        {
            _rolUserData = rolUserData ?? throw new ArgumentNullException(nameof(rolUserData));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<RolUserDTO>> GetAllRolUsersAsync()
        {
            try
            {
                var rolUsers = await _rolUserData.GetAllAsyncSQL();
                return MapToDtoList(rolUsers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los RolUsers.");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de RolUsers.", ex);
            }
        }

        public async Task<RolUserDTO> GetRolUserByIdAsync(int id)
        {
            if (id <= 0)
                throw new ValidationException("id", "El ID debe ser mayor que cero.");

            try
            {
                var rolUser = await _rolUserData.GetByIdAsyncSQL(id);
                if (rolUser == null)
                    throw new EntityNotFoundException("RolUser", id);

                return MapToDTO(rolUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el RolUser con ID: {RolUserId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el RolUser con ID {id}.", ex);
            }
        }

        public async Task<RolUserDTO> CreateRolUserAsync(RolUserDTO dto)
        {
            try
            {
                ValidateRolUser(dto);
                var entity = MapToEntity(dto);
                var created = await _rolUserData.CreateAsyncSQL(entity);
                return MapToDTO(created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el RolUser.");
                throw new ExternalServiceException("Base de datos", "Error al crear el RolUser.", ex);
            }
        }

        public async Task<bool> UpdateRolUserAsync(RolUserDTO dto)
        {
            try
            {
                ValidateRolUser(dto);
                var existing = await _rolUserData.GetByIdAsyncSQL(dto.Id);
                if (existing == null)
                    throw new EntityNotFoundException("RolUser", dto.Id);

                existing.UserId = dto.UserId;
                existing.RolId = dto.RolId;

                return await _rolUserData.UpdateAsyncSQL(existing);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el RolUser con ID: {RolUserId}", dto.Id);
                throw new ExternalServiceException("Base de datos", "Error al actualizar el RolUser.", ex);
            }
        }

        public async Task<bool> DeleteRolUserAsync(int id)
        {
            try
            {
                var existing = await _rolUserData.GetByIdAsyncSQL(id);
                if (existing == null)
                    throw new EntityNotFoundException("RolUser", id);

                return await _rolUserData.DeleteAsyncSQL(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el RolUser con ID: {RolUserId}", id);
                throw new ExternalServiceException("Base de datos", "Error al eliminar el RolUser.", ex);
            }
        }

        private void ValidateRolUser(RolUserDTO dto)
        {
            if (dto == null)
                throw new ValidationException("El objeto RolUser no puede ser nulo.");

            if (dto.UserId <= 0)
                throw new ValidationException("UserId", "El ID del usuario debe ser mayor que cero.");

            if (dto.RolId <= 0)
                throw new ValidationException("RolId", "El ID del rol debe ser mayor que cero.");
        }

        private RolUserDTO MapToDTO(RolUser entity)
        {
            return new RolUserDTO
            {
                Id = entity.Id,
                UserId = entity.UserId,
                RolId = entity.RolId
            };
        }

        private RolUser MapToEntity(RolUserDTO dto)
        {
            return new RolUser
            {
                Id = dto.Id,
                UserId = dto.UserId,
                RolId = dto.RolId
            };
        }

        private IEnumerable<RolUserDTO> MapToDtoList(IEnumerable<RolUser> list)
        {
            foreach (var item in list)
                yield return MapToDTO(item);
        }
    }
}
