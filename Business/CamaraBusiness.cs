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
    public class CamaraBusiness
    {
        private readonly CamaraData _CamaraData;
        private readonly ILogger<CamaraBusiness> _logger;

        public CamaraBusiness(CamaraData CamaraData, ILogger<CamaraBusiness> logger)
        {
            _CamaraData = CamaraData ?? throw new ArgumentNullException(nameof(CamaraData));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<CamaraDTO>> GetAllCamarasAsync()
        {
            try
            {
                var camaras = await _CamaraData.GetAllAsyncSQL();
                return MapToDtoList(camaras);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las cámaras.");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de cámaras.", ex);
            }
        }

        public async Task<CamaraDTO> GetCamaraByIdAsync(int id)
        {
            if (id <= 0)
                throw new ValidationException("id", "El ID debe ser mayor que cero.");

            try
            {
                var camara = await _CamaraData.GetByIdAsyncSQL(id);
                if (camara == null)
                    throw new EntityNotFoundException("Camara", id);

                return MapToDTO(camara);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la cámara con ID: {CamaraId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la cámara con ID {id}.", ex);
            }
        }

        public async Task<CamaraDTO> CreateCamaraAsync(CamaraDTO dto)
        {
            try
            {
                ValidateCamara(dto);
                var entity = MapToEntity(dto);
                var created = await _CamaraData.CreateAsyncSQL(entity);
                return MapToDTO(created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la cámara: {Name}", dto?.name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear la cámara.", ex);
            }
        }

        public async Task<bool> UpdateCamaraAsync(CamaraDTO dto)
        {
            try
            {
                ValidateCamara(dto);
                var existing = await _CamaraData.GetByIdAsyncSQL(dto.id);
                if (existing == null)
                    throw new EntityNotFoundException("Camara", dto.id);

                existing.name = dto.name;
                existing.nightvisioninfrared = dto.nightvisioninfrared;
                existing.highresolution = dto.highresolution;
                existing.infraredlighting = dto.infraredlighting;
                existing.optimizedangleofvision = dto.optimizedangleofvision;
                existing.highshutterspeed = dto.highshutterspeed;

                return await _CamaraData.UpdateAsyncSQL(existing);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la cámara con ID: {CamaraId}", dto.id);
                throw new ExternalServiceException("Base de datos", "Error al actualizar la cámara.", ex);
            }
        }

        public async Task<bool> DeleteCamaraAsync(int id)
        {
            try
            {
                var existing = await _CamaraData.GetByIdAsyncSQL(id);
                if (existing == null)
                    throw new EntityNotFoundException("Camara", id);

                return await _CamaraData.DeleteAsyncSQL(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la cámara con ID: {CamaraId}", id);
                throw new ExternalServiceException("Base de datos", "Error al eliminar la cámara.", ex);
            }
        }

        private void ValidateCamara(CamaraDTO dto)
        {
            if (dto == null)
                throw new ValidationException("El objeto cámara no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(dto.name))
                throw new ValidationException("name", "El nombre de la cámara es obligatorio.");
        }

        private CamaraDTO MapToDTO(Camara camara)
        {
            return new CamaraDTO
            {
                id = camara.id,
                name = camara.name,
                nightvisioninfrared = camara.nightvisioninfrared,
                highresolution = camara.highresolution,
                infraredlighting = camara.infraredlighting,
                optimizedangleofvision = camara.optimizedangleofvision,
                highshutterspeed = camara.highshutterspeed
            };
        }

        private Camara MapToEntity(CamaraDTO dto)
        {
            return new Camara
            {
                id = dto.id,
                name = dto.name,
                nightvisioninfrared = dto.nightvisioninfrared,
                highresolution = dto.highresolution,
                infraredlighting = dto.infraredlighting,
                optimizedangleofvision = dto.optimizedangleofvision,
                highshutterspeed = dto.highshutterspeed
            };
        }

        private IEnumerable<CamaraDTO> MapToDtoList(IEnumerable<Camara> entities)
        {
            var list = new List<CamaraDTO>();
            foreach (var item in entities)
            {
                list.Add(MapToDTO(item));
            }
            return list;
        }
    }
}
