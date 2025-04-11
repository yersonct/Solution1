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
    public class VehicleBusiness
    {
        private readonly VehicleData _vehicleData;
        private readonly ILogger<VehicleBusiness> _logger;

        public VehicleBusiness(VehicleData vehicleData, ILogger<VehicleBusiness> logger)
        {
            _vehicleData = vehicleData ?? throw new ArgumentNullException(nameof(vehicleData));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<VehicleDTO>> GetAllAsync()
        {
            var vehicles = await _vehicleData.GetAllAsync();
            return vehicles.Select(v => new VehicleDTO
            {
                Id = v.id,
                Plate = v.plate,
                Color = v.color,
                Id_Client = v.id_client
            });
        }

        public async Task<VehicleDTO> GetByIdAsync(int id)
        {
            var vehicle = await _vehicleData.GetByIdAsync(id);
            if (vehicle == null)
                throw new EntityNotFoundException($"Vehículo con ID {id} no encontrado.");

            return new VehicleDTO
            {
                Id = vehicle.id,
                Plate = vehicle.plate,
                Color = vehicle.color,
                Id_Client = vehicle.id_client
            };
        }

        public async Task<VehicleDTO> CreateAsync(VehicleCreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.plate))
                throw new ValidationException("La placa del vehículo no puede estar vacía.");

            var entity = new Vehicle
            {
                plate = dto.plate,
                color = dto.color,
                id_client = dto.id_client
            };

            var created = await _vehicleData.CreateAsync(entity);

            return new VehicleDTO
            {
                Id = created.id,
                Plate = created.plate,
                Color = created.color,
                Id_Client = created.id_client
            };
        }

        public async Task<bool> UpdateAsync(int id, VehicleCreateDTO dto)
        {
            var vehicle = await _vehicleData.GetByIdAsync(id);
            if (vehicle == null)
                throw new EntityNotFoundException($"Vehículo con ID {id} no encontrado.");

            if (string.IsNullOrWhiteSpace(dto.plate))
                throw new ValidationException("La placa del vehículo no puede estar vacía.");

            vehicle.plate = dto.plate;
            vehicle.color = dto.color;
            vehicle.id_client = dto.id_client;

            return await _vehicleData.UpdateAsync(vehicle);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var vehicle = await _vehicleData.GetByIdAsync(id);
            if (vehicle == null)
                throw new EntityNotFoundException($"Vehículo con ID {id} no encontrado.");

            return await _vehicleData.DeleteAsync(id);
        }
    }
}
