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
    public class TypeVehicleBusiness
    {
        private readonly TypeVehicleData _typeVehicleData;
        private readonly ILogger<TypeVehicleBusiness> _logger;

        public TypeVehicleBusiness(TypeVehicleData typeVehicleData, ILogger<TypeVehicleBusiness> logger)
        {
            _typeVehicleData = typeVehicleData ?? throw new ArgumentNullException(nameof(typeVehicleData));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<TypeVehicleDTO>> GetAllTypeVehiclesAsync()
        {
            try
            {
                var typeVehicles = await _typeVehicleData.GetAllAsyncSQL();
                return MapToDtoList(typeVehicles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los TypeVehicles.");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de TypeVehicles.", ex);
            }
        }

        public async Task<TypeVehicleDTO> GetTypeVehicleByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ValidationException("id", "El ID debe ser mayor que cero.");
            }

            try
            {
                var typeVehicle = await _typeVehicleData.GetByIdAsyncSQL(id);
                if (typeVehicle == null)
                {
                    throw new EntityNotFoundException("TypeVehicle", id);
                }
                return MapToDTO(typeVehicle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener TypeVehicle con ID: {TypeVehicleId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar TypeVehicle con ID {id}.", ex);
            }
        }

        public async Task<TypeVehicleDTO> CreateTypeVehicleAsync(TypeVehicleDTO typeVehicleDTO)
        {
            try
            {
                ValidateTypeVehicle(typeVehicleDTO);
                var typeVehicle = MapToEntity(typeVehicleDTO);
                var createdTypeVehicle = await _typeVehicleData.CreateAsyncSQL(typeVehicle);
                return MapToDTO(createdTypeVehicle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo TypeVehicle: {TypeVehicleName}", typeVehicleDTO?.name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el TypeVehicle.", ex);
            }
        }

        public async Task<bool> UpdateTypeVehicleAsync(TypeVehicleDTO typeVehicleDTO)
        {
            try
            {
                ValidateTypeVehicle(typeVehicleDTO);
                var existingTypeVehicle = await _typeVehicleData.GetByIdAsyncSQL(typeVehicleDTO.id);
                if (existingTypeVehicle == null)
                {
                    throw new EntityNotFoundException("TypeVehicle", typeVehicleDTO.id);
                }

                existingTypeVehicle.name = typeVehicleDTO.name;

                return await _typeVehicleData.UpdateAsyncSQL(existingTypeVehicle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar TypeVehicle con ID: {TypeVehicleId}", typeVehicleDTO.id);
                throw new ExternalServiceException("Base de datos", "Error al actualizar el TypeVehicle.", ex);
            }
        }

        public async Task<bool> DeleteTypeVehicleAsync(int id)
        {
            try
            {
                var existingTypeVehicle = await _typeVehicleData.GetByIdAsyncSQL(id);
                if (existingTypeVehicle == null)
                {
                    throw new EntityNotFoundException("TypeVehicle", id);
                }
                return await _typeVehicleData.DeleteAsyncSQL(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar TypeVehicle con ID: {TypeVehicleId}", id);
                throw new ExternalServiceException("Base de datos", "Error al eliminar el TypeVehicle.", ex);
            }
        }

        private void ValidateTypeVehicle(TypeVehicleDTO typeVehicleDTO)
        {
            if (typeVehicleDTO == null)
            {
                throw new ValidationException("El objeto TypeVehicle no puede ser nulo.");
            }
            if (string.IsNullOrWhiteSpace(typeVehicleDTO.name))
            {
                throw new ValidationException("Name", "El nombre del TypeVehicle es obligatorio.");
            }
        }

        private TypeVehicleDTO MapToDTO(TypeVehicle typeVehicle)
        {
            return new TypeVehicleDTO
            {
                id = typeVehicle.id,
                name = typeVehicle.name
            };
        }

        private TypeVehicle MapToEntity(TypeVehicleDTO typeVehicleDTO)
        {
            return new TypeVehicle
            {
                id = typeVehicleDTO.id,
                name = typeVehicleDTO.name
            };
        }

        private IEnumerable<TypeVehicleDTO> MapToDtoList(IEnumerable<TypeVehicle> typeVehicles)
        {
            var typeVehicleDTOs = new List<TypeVehicleDTO>();
            foreach (var typeVehicle in typeVehicles)
            {
                typeVehicleDTOs.Add(MapToDTO(typeVehicle));
            }
            return typeVehicleDTOs;
        }
    }
}
