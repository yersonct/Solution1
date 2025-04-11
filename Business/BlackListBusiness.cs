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
    public class BlackListBusiness
    {
        private readonly BlackListData _blackListData;
        private readonly ILogger<BlackListBusiness> _logger;

        public BlackListBusiness(BlackListData blackListData, ILogger<BlackListBusiness> logger)
        {
            _blackListData = blackListData ?? throw new ArgumentNullException(nameof(blackListData));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<BlackListDTO>> GetAllAsync()
        {
            try
            {
                var blacklists = await _blackListData.GetAllAsyncSQL();
                return MapToDtoList(blacklists);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los registros de la lista negra.");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista negra.", ex);
            }
        }

        public async Task<BlackListDTO> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ValidationException("Id", "El ID debe ser mayor que cero.");

            try
            {
                var blacklist = await _blackListData.GetByIdAsyncSQL(id);
                if (blacklist == null)
                    throw new EntityNotFoundException("BlackList", id);

                return MapToDTO(blacklist);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el registro de lista negra con ID {BlackListId}", id);
                throw new ExternalServiceException("Base de datos", "Error al recuperar el registro de la lista negra.", ex);
            }
        }

        public async Task<BlackListDTO> CreateAsync(BlackListDTO dto)
        {
            try
            {
                Validate(dto);
                var entity = MapToEntity(dto);
                var created = await _blackListData.CreateAsyncSQL(entity);
                return MapToDTO(created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el registro de lista negra para ClientId {ClientId}", dto.id_client);
                throw new ExternalServiceException("Base de datos", "Error al crear el registro de la lista negra.", ex);
            }
        }

        public async Task<bool> UpdateAsync(BlackListDTO dto)
        {
            try
            {
                Validate(dto);
                var existing = await _blackListData.GetByIdAsyncSQL(dto.id);
                if (existing == null)
                    throw new EntityNotFoundException("BlackList", dto.id);

                existing.reason = dto.reason;
                existing.restrictiondate = dto.restrictiondate;
                existing.id_client = dto.id_client;

                return await _blackListData.UpdateAsyncSQL(existing);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el registro de lista negra con ID {BlackListId}", dto.id);
                throw new ExternalServiceException("Base de datos", "Error al actualizar el registro de la lista negra.", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var existing = await _blackListData.GetByIdAsyncSQL(id);
                if (existing == null)
                    throw new EntityNotFoundException("BlackList", id);

                return await _blackListData.DeleteAsyncSQL(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el registro de lista negra con ID {BlackListId}", id);
                throw new ExternalServiceException("Base de datos", "Error al eliminar el registro de la lista negra.", ex);
            }
        }

        private void Validate(BlackListDTO dto)
        {
            if (dto == null)
                throw new ValidationException("El objeto BlackList no puede ser nulo.");
            if (string.IsNullOrWhiteSpace(dto.reason))
                throw new ValidationException("Reason", "La razón es obligatoria.");
        }

        private BlackListDTO MapToDTO(BlackList entity)
        {
            return new BlackListDTO
            {
                id = entity.id,
                reason = entity.reason,
                restrictiondate = entity.restrictiondate,
                id_client = entity.id_client
            };
        }

        private BlackList MapToEntity(BlackListDTO dto)
        {
            return new BlackList
            {
                id = dto.id,
                reason = dto.reason,
                restrictiondate = dto.restrictiondate,
                id_client = dto.id_client
            };
        }

        private IEnumerable<BlackListDTO> MapToDtoList(IEnumerable<BlackList> entities)
        {
            var list = new List<BlackListDTO>();
            foreach (var item in entities)
            {
                list.Add(MapToDTO(item));
            }
            return list;
        }
    }
}
