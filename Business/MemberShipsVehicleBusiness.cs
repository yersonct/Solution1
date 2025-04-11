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
    public class MembershipsVehicleBusiness
    {
        private readonly MembershipsVehicleData _data;
        private readonly ILogger<MembershipsVehicleBusiness> _logger;

        public MembershipsVehicleBusiness(MembershipsVehicleData data, ILogger<MembershipsVehicleBusiness> logger)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<MembershipsVehicleDTO>> GetAllAsync()
        {
            var list = await _data.GetAllAsync();
            return list.Select(m => new MembershipsVehicleDTO
            {
                id = m.id,
                id_memberships = m.id_memberships,
                id_vehicle = m.id_vehicle
            });
        }

        public async Task<MembershipsVehicleDTO> GetByIdAsync(int id)
        {
            var entity = await _data.GetByIdAsync(id);
            if (entity == null)
                throw new EntityNotFoundException($"Membresía-Vehículo con ID {id} no encontrada.");

            return new MembershipsVehicleDTO
            {
                id = entity.id,
                id_memberships = entity.id_memberships,
                id_vehicle = entity.id_vehicle
            };
        }

        public async Task<MembershipsVehicleDTO> CreateAsync(MemberShipsVehicleCreateDTO dto)
        {
            if (dto.membershipsid <= 0 || dto.vehicleid <= 0)
                throw new ValidationException("ID de membresía y vehículo deben ser válidos.");

            var entity = new MembershipsVehicle
            {
                id_memberships = dto.membershipsid,
                id_vehicle = dto.vehicleid
            };

            var created = await _data.CreateAsync(entity);

            return new MembershipsVehicleDTO
            {
                id = created.id,
                id_memberships = created.id_memberships,
                id_vehicle = created.id_vehicle
            };
        }

        public async Task<bool> UpdateAsync(int id, MemberShipsVehicleCreateDTO dto)
        {
            var existing = await _data.GetByIdAsync(id);
            if (existing == null)
                throw new EntityNotFoundException($"Membresía-Vehículo con ID {id} no encontrada.");

            if (dto.membershipsid <= 0 || dto.vehicleid <= 0)
                throw new ValidationException("ID de membresía y vehículo deben ser válidos.");

            existing.id_memberships = dto.membershipsid;
            existing.id_vehicle = dto.vehicleid;

            return await _data.UpdateAsync(existing);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _data.GetByIdAsync(id);
            if (entity == null)
                throw new EntityNotFoundException($"Membresía-Vehículo con ID {id} no encontrada.");

            return await _data.DeleteAsync(id);
        }
    }
}
