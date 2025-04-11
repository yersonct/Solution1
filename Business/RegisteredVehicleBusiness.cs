using System;
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
    public class RegisteredVehicleBusiness
    {
        private readonly RegisteredVehicleData _registeredVehicleData;
        private readonly ILogger<RegisteredVehicleBusiness> _logger;

        public RegisteredVehicleBusiness(RegisteredVehicleData registeredVehicleData, ILogger<RegisteredVehicleBusiness> logger)
        {
            _registeredVehicleData = registeredVehicleData ?? throw new ArgumentNullException(nameof(registeredVehicleData));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<RegisteredVehicleDTO>> GetAllAsync()
        {
            var list = await _registeredVehicleData.GetAllAsync();
            return list.Select(rv => new RegisteredVehicleDTO
            {
                id = rv.id,
                entrydatetime = rv.entrydatetime,
                exitdatetime = rv.exitdatetime,
                id_vehicle = rv.id_vehicle
            });
        }

        public async Task<RegisteredVehicleDTO> GetByIdAsync(int id)
        {
            var entity = await _registeredVehicleData.GetByIdAsync(id);
            if (entity == null)
                throw new EntityNotFoundException($"Registro con ID {id} no encontrado.");

            return new RegisteredVehicleDTO
            {
                id = entity.id,
                entrydatetime = entity.entrydatetime,
                exitdatetime = entity.exitdatetime,
                id_vehicle = entity.id_vehicle
            };
        }

        public async Task<RegisteredVehicleDTO> CreateAsync(RegisteredVehicleCreateDTO dto)
        {
            if (dto.entrydatetime == default)
                throw new ValidationException("El campo EntryTime es requerido.");

            if (dto.id_vehicle <= 0)
                throw new ValidationException("El ID del vehículo debe ser válido.");

            var entity = new RegisteredVehicle
            {
                entrydatetime = dto.entrydatetime,
                exitdatetime = dto.exitdatetime,
                id_vehicle = dto.id_vehicle
            };

            var created = await _registeredVehicleData.CreateAsync(entity);

            return new RegisteredVehicleDTO
            {
                id = created.id,
                entrydatetime = created.entrydatetime,
                exitdatetime = created.exitdatetime,
                id_vehicle = created.id_vehicle
            };
        }

        public async Task<bool> UpdateAsync(int id, RegisteredVehicleCreateDTO dto)
        {
            var entity = await _registeredVehicleData.GetByIdAsync(id);
            if (entity == null)
                throw new EntityNotFoundException($"Registro con ID {id} no encontrado.");

            if (dto.entrydatetime == default)
                throw new ValidationException("El campo EntryTime es requerido.");

            if (dto.id_vehicle <= 0)
                throw new ValidationException("El ID del vehículo debe ser válido.");

            entity.entrydatetime = dto.entrydatetime;
            entity.exitdatetime = dto.exitdatetime;
            entity.id_vehicle = dto.id_vehicle;

            return await _registeredVehicleData.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _registeredVehicleData.GetByIdAsync(id);
            if (entity == null)
                throw new EntityNotFoundException($"Registro con ID {id} no encontrado.");

            return await _registeredVehicleData.DeleteAsync(id);
        }
    }
}
