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
    public class TypeRatesBusiness
    {
        private readonly TypeRatesData _TypeRatesData;
        private readonly ILogger<TypeRatesBusiness> _logger;

        public TypeRatesBusiness(TypeRatesData TypeRatesData, ILogger<TypeRatesBusiness> logger)
        {
            _TypeRatesData = TypeRatesData ?? throw new ArgumentNullException(nameof(TypeRatesData));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<TypeRatesDTO>> GetAllTypeRatessAsync()
        {
            try
            {
                var TypeRates = await _TypeRatesData.GetAllAsyncSQL();

                return MapToDtoList(TypeRates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los TypeRatesas.");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de TypeRatesa.", ex);
            }
        }

        public async Task<TypeRatesDTO> GetTypeRatesByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ValidationException("id", "El ID del usuario debe ser mayor que cero.");
            }

            try
            {
                var TypeRates = await _TypeRatesData.GetByIdAsyncSQL(id);
                if (TypeRates == null)
                {
                    throw new EntityNotFoundException("TypeRates", id);
                }
                return MapToDTO(TypeRates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario con ID: {TypeRatesId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el usuario con ID {id}.", ex);
            }
        }

        public async Task<TypeRatesDTO> CreateTypeRatesAsync(TypeRatesDTO TypeRatesDTO)
        {
            try
            {
                ValidateTypeRates(TypeRatesDTO);
                var TypeRatesa = MapToEntity(TypeRatesDTO);
                var createdTypeRates = await _TypeRatesData.CreateAsyncSQL(TypeRatesa);
                return MapToDTO(createdTypeRates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo usuario: {TypeRatesname}", TypeRatesDTO?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el usuario.", ex);
            }
        }

        public async Task<bool> UpdateTypeRatesAsync(TypeRatesDTO TypeRatesDTO)
        {
            try
            {
                ValidateTypeRates(TypeRatesDTO);
                var existingTypeRates = await _TypeRatesData.GetByIdAsyncSQL(TypeRatesDTO.Id);
                if (existingTypeRates == null)
                {
                    throw new EntityNotFoundException("TypeRates", TypeRatesDTO.Id);
                }

                existingTypeRates.Name = TypeRatesDTO.Name;
                existingTypeRates.Price = TypeRatesDTO.Price;
              

                return await _TypeRatesData.UpdateAsyncSQL(existingTypeRates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el usuario con ID: {TypeRatesId}", TypeRatesDTO.Id);
                throw new ExternalServiceException("Base de datos", "Error al actualizar el usuario.", ex);
            }
        }

        public async Task<bool> DeleteTypeRatesAsync(int id)
        {
            try
            {
                var existingTypeRates = await _TypeRatesData.GetByIdAsyncSQL(id);
                if (existingTypeRates == null)
                {
                    throw new EntityNotFoundException("TypeRates", id);
                }
                return await _TypeRatesData.DeleteAsyncSQL(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el usuario con ID: {TypeRatesId}", id);
                throw new ExternalServiceException("Base de datos", "Error al eliminar el TypeRates.", ex);
            }
        }

        private void ValidateTypeRates(TypeRatesDTO TypeRatesDTO)
        {
            if (TypeRatesDTO == null)
            {
                throw new ValidationException("El objeto TypeRatesas no puede ser nulo.");
            }
            if (string.IsNullOrWhiteSpace(TypeRatesDTO.Name))
            {
                throw new ValidationException("Name", "El nombre de usuario es obligatorio.");
            }
        }

        private TypeRatesDTO MapToDTO(TypeRates TypeRates)
        {
            return new TypeRatesDTO
            {
                Id = TypeRates.Id,
                Name = TypeRates.Name,
                Price = TypeRates.Price


            };
        }

        private TypeRates MapToEntity(TypeRatesDTO TypeRatesDTO)
        {
            return new TypeRates
            {
                Id = TypeRatesDTO.Id,
                Name = TypeRatesDTO.Name,
                Price = TypeRatesDTO.Price  
         
            };
        }
        private IEnumerable<TypeRatesDTO> MapToDtoList(IEnumerable<TypeRates> TypeRatess)
        {
            var TypeRatesDto = new List<TypeRatesDTO>();
            foreach (var TypeRates in TypeRatess)
            {
                TypeRatesDto.Add(MapToDTO(TypeRates));
            }
            return TypeRatesDto;
        }


    }
}
