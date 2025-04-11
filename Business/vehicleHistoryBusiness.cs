using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Entity.DTOs;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    public class VehicleHistoryBusiness
    {
        private readonly VehicleHistoryData _vehicleHistoryData;
        private readonly ILogger<VehicleHistoryBusiness> _logger;

        public VehicleHistoryBusiness(VehicleHistoryData vehicleHistoryData, ILogger<VehicleHistoryBusiness> logger)
        {
            _vehicleHistoryData = vehicleHistoryData ?? throw new ArgumentNullException(nameof(vehicleHistoryData));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Entity.DTOs.VehicleHistoryDTO>> GetAllAsync()
        {
            try
            {
                return await _vehicleHistoryData.GetAllAsyncLINQ(); // O GetAllAsyncSQL(), elige la implementación que prefieras
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los registros de VehicleHistory.");
                throw;
            }
        }

        public async Task<Entity.DTOs.VehicleHistoryDTO> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _vehicleHistoryData.GetByIdAsyncLINQ(id); // O GetByIdAsyncSQL(id)
                if (entity == null)
                {
                    throw new EntityNotFoundException($"Registro de VehicleHistory con ID {id} no encontrado.");
                }
                return entity;
            }
            catch (EntityNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el registro de VehicleHistory con ID {Id}.", id);
                throw;
            }
        }

        public async Task<Entity.DTOs.VehicleHistoryDTO> CreateAsync(VehicleHistoryCreateDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    throw new ArgumentNullException(nameof(dto));
                }

                // Aquí podrías agregar validaciones de negocio más complejas si son necesarias
                if (dto.RegisteredVehicleId <= 0)
                {
                    throw new ValidationException("El RegisteredVehicleId debe ser un valor válido.");
                }
                if (dto.TypeVehicleId <= 0)
                {
                    throw new ValidationException("El TypeVehicleId debe ser un valor válido.");
                }
                // TotalTime ya es TimeSpan, pero podrías validar rangos si es necesario

                var createdEntity = await _vehicleHistoryData.CreateAsyncLINQ(dto); // O CreateAsyncSQL(dto)

                // Opcional: Puedes recuperar la entidad completa después de la creación si necesitas más detalles
                return await _vehicleHistoryData.GetByIdAsyncLINQ(createdEntity.id);
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo registro de VehicleHistory.", dto);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(int id, VehicleHistoryCreateDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    throw new ArgumentNullException(nameof(dto));
                }

                // Aquí podrías agregar validaciones de negocio antes de la actualización
                if (dto.RegisteredVehicleId <= 0)
                {
                    throw new ValidationException("El RegisteredVehicleId debe ser un valor válido.");
                }
                if (dto.TypeVehicleId <= 0)
                {
                    throw new ValidationException("El TypeVehicleId debe ser un valor válido.");
                }

                var existingEntity = await _vehicleHistoryData.GetByIdAsyncLINQ(id); // Verificar si existe
                if (existingEntity == null)
                {
                    throw new EntityNotFoundException($"Registro de VehicleHistory con ID {id} no encontrado para actualizar.");
                }

                return await _vehicleHistoryData.UpdateAsyncLINQ(id, dto); // O UpdateAsyncSQL(id, dto)
            }
            catch (EntityNotFoundException)
            {
                throw;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el registro de VehicleHistory con ID {Id}.", id, dto);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var existingEntity = await _vehicleHistoryData.GetByIdAsyncLINQ(id); // Verificar si existe
                if (existingEntity == null)
                {
                    throw new EntityNotFoundException($"Registro de VehicleHistory con ID {id} no encontrado para eliminar.");
                }

                return await _vehicleHistoryData.DeleteAsyncLINQ(id); // O DeleteAsyncSQL(id)
            }
            catch (EntityNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el registro de VehicleHistory con ID {Id}.", id);
                throw;
            }
        }
    }
}