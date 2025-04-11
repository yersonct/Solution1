using System;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    public class RolBusiness
    {
        private readonly RolData _RolData;
        private readonly ILogger<RolBusiness> _logger;

        public RolBusiness(RolData RolData, ILogger<RolBusiness> logger)
        {
            _RolData = RolData ?? throw new ArgumentNullException(nameof(RolData));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<RolDTO>> GetAllRolsAsync()
        {
            try
            {
                var Rol = await _RolData.GetAllAsyncSQL();

                return MapToDtoList(Rol);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los Rolas.");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de Rola.", ex);
            }
        }

        public async Task<RolDTO> GetRolByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ValidationException("id", "El ID del usuario debe ser mayor que cero.");
            }

            try
            {
                var Rol = await _RolData.GetByIdAsyncSQL(id);
                if (Rol == null)
                {
                    throw new EntityNotFoundException("Rol", id);
                }
                return MapToDTO(Rol);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario con ID: {RolId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el usuario con ID {id}.", ex);
            }
        }

        public async Task<RolDTO> CreateRolAsync(RolDTO RolDTO)
        {
            try
            {
                ValidateRol(RolDTO);
                var Rola = MapToEntity(RolDTO);
                var createdRol = await _RolData.CreateAsyncSQL(Rola);
                return MapToDTO(createdRol);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo usuario: {Rolname}", RolDTO?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el usuario.", ex);
            }
        }

        public async Task<bool> UpdateRolAsync(RolDTO RolDTO)
        {
            try
            {
                ValidateRol(RolDTO);
                var existingRol = await _RolData.GetByIdAsyncSQL(RolDTO.Id);
                if (existingRol == null)
                {
                    throw new EntityNotFoundException("Rol", RolDTO.Id);
                }

                existingRol.Name = RolDTO.Name;
                existingRol.Description = RolDTO.Description;


                return await _RolData.UpdateAsyncSQL(existingRol);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el usuario con ID: {RolId}", RolDTO.Id);
                throw new ExternalServiceException("Base de datos", "Error al actualizar el usuario.", ex);
            }
        }

        public async Task<bool> DeleteRolAsync(int id)
        {
            try
            {
                var existingRol = await _RolData.GetByIdAsyncSQL(id);
                if (existingRol == null)
                {
                    throw new EntityNotFoundException("Rol", id);
                }
                return await _RolData.DeleteAsyncSQL(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el usuario con ID: {RolId}", id);
                throw new ExternalServiceException("Base de datos", "Error al eliminar el Rol.", ex);
            }
        }

        private void ValidateRol(RolDTO RolDTO)
        {
            if (RolDTO == null)
            {
                throw new ValidationException("El objeto Rolas no puede ser nulo.");
            }
            if (string.IsNullOrWhiteSpace(RolDTO.Name))
            {
                throw new ValidationException("Name", "El nombre de usuario es obligatorio.");
            }
        }

        private RolDTO MapToDTO(Rol Rol)
        {
            return new RolDTO
            {
                Id = Rol.Id,
                Name = Rol.Name,
                Description = Rol.Description

            };
        }

        private Rol MapToEntity(RolDTO RolDTO)
        {
            return new Rol
            {
                Id = RolDTO.Id,
                Name = RolDTO.Name,
                Description= RolDTO.Description

            };
        }
        private IEnumerable<RolDTO> MapToDtoList(IEnumerable<Rol> Rols)
        {
            var RolDto = new List<RolDTO>();
            foreach (var Rol in Rols)
            {
                RolDto.Add(MapToDTO(Rol));
            }
            return RolDto;
        }


    }
}
