using Business.Interfaces;
using Data.Interfaces;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class MembershipsVehicleService : IMembershipsVehicleService
    {
        private readonly IMembershipsVehicleRepository _membershipsVehicleRepository;
        private readonly ILogger<MembershipsVehicleService> _logger;

        public MembershipsVehicleService(IMembershipsVehicleRepository membershipsVehicleRepository, ILogger<MembershipsVehicleService> logger)
        {
            _membershipsVehicleRepository = membershipsVehicleRepository ?? throw new ArgumentNullException(nameof(membershipsVehicleRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<MembershipsVehicle>> GetAllMembershipsVehiclesAsync()
        {
            return await _membershipsVehicleRepository.GetAllAsync(); // Obtiene solo los activos
        }

        public async Task<MembershipsVehicle?> GetMembershipsVehicleByIdAsync(int id)
        {
            return await _membershipsVehicleRepository.GetByIdAsync(id); // Obtiene solo el activo
        }

        public async Task<MembershipsVehicle> CreateMembershipsVehicleAsync(MembershipsVehicle membershipsVehicle)
        {
            // Aquí podrías agregar lógica de negocio antes de crear la relación
            return await _membershipsVehicleRepository.AddAsync(membershipsVehicle);
        }

        public async Task<bool> UpdateMembershipsVehicleAsync(MembershipsVehicle membershipsVehicle)
        {
            // Aquí podrías agregar lógica de negocio antes de actualizar la relación
            return await _membershipsVehicleRepository.UpdateAsync(membershipsVehicle); // Ya considera solo los activos en el repositorio
        }

        public async Task<bool> DeleteMembershipsVehicleAsync(int id)
        {
            // Aquí podrías agregar lógica de negocio antes de eliminar la relación
            return await _membershipsVehicleRepository.DeleteAsync(id); // Realiza la eliminación lógica
        }
    }
}