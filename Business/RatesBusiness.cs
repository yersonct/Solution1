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
    public class RatesBusiness
    {
        private readonly RatesData _RatesData;
        private readonly ILogger<RatesBusiness> _logger;

        public RatesBusiness(RatesData RatesData, ILogger<RatesBusiness> logger)
        {
            _RatesData = RatesData ?? throw new ArgumentNullException(nameof(RatesData));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<RatesDTO>> GetAllRatessAsync()
        {
            try
            {
                var rates = await _RatesData.GetAllAsyncSQL();
                return MapToDtoList(rates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las tarifas.");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de tarifas.", ex);
            }
        }

        public async Task<RatesDTO> GetRatesByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ValidationException("id", "El ID de la tarifa debe ser mayor que cero.");
            }

            try
            {
                var rates = await _RatesData.GetByIdAsyncSQL(id);
                if (rates == null)
                {
                    throw new EntityNotFoundException("Rates", id);
                }
                return MapToDTO(rates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la tarifa con ID: {RatesId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la tarifa con ID {id}.", ex);
            }
        }

        public async Task<RatesDTO> CreateRatesAsync(RatesDTO dto)
        {
            try
            {
                ValidateRates(dto);
                var entity = MapToEntity(dto);
                var created = await _RatesData.CreateAsyncSQL(entity);
                return MapToDTO(created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear una nueva tarifa: {TypeRates}", dto?.typerates ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear la tarifa.", ex);
            }
        }

        public async Task<bool> UpdateRatesAsync(RatesDTO dto)
        {
            try
            {
                ValidateRates(dto);
                var existing = await _RatesData.GetByIdAsyncSQL(dto.id);
                if (existing == null)
                {
                    throw new EntityNotFoundException("Rates", dto.id);
                }

                existing.amount = dto.amount;
                existing.startduration = dto.startduration;
                existing.endduration = dto.endduration;
                existing.id_typerates = dto.id_typerates;

                return await _RatesData.UpdateAsyncSQL(existing);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la tarifa con ID: {RatesId}", dto.id);
                throw new ExternalServiceException("Base de datos", "Error al actualizar la tarifa.", ex);
            }
        }

        public async Task<bool> DeleteRatesAsync(int id)
        {
            try
            {
                var existing = await _RatesData.GetByIdAsyncSQL(id);
                if (existing == null)
                {
                    throw new EntityNotFoundException("Rates", id);
                }
                return await _RatesData.DeleteAsyncSQL(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la tarifa con ID: {RatesId}", id);
                throw new ExternalServiceException("Base de datos", "Error al eliminar la tarifa.", ex);
            }
        }

        private void ValidateRates(RatesDTO dto)
        {
            if (dto == null)
            {
                throw new ValidationException("El objeto de tarifa no puede ser nulo.");
            }
            if (string.IsNullOrWhiteSpace(dto.typerates))
            {
                throw new ValidationException("TypeRates", "El tipo de tarifa es obligatorio.");
            }
        }

        private RatesDTO MapToDTO(Rates rates)
        {
            return new RatesDTO
            {
                id = rates.id,
                amount = rates.amount,
                startduration = rates.startduration,
                endduration = rates.endduration,
                id_typerates = rates.id_typerates
            };
        }

        private Rates MapToEntity(RatesDTO dto)
        {
            return new Rates
            {
                id = dto.id,
                amount = dto.amount,
                startduration = dto.startduration,
                endduration = dto.endduration,
                id_typerates = dto.id_typerates
            };
        }

        private IEnumerable<RatesDTO> MapToDtoList(IEnumerable<Rates> list)
        {
            var result = new List<RatesDTO>();
            foreach (var item in list)
            {
                result.Add(MapToDTO(item));
            }
            return result;
        }
    }
}
