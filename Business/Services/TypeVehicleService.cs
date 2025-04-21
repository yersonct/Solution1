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
    public class TypeVehicleService : ITypeVehicleService
    {
        private readonly ITypeVehicleRepository _typeVehicleRepository;
        private readonly ILogger<TypeVehicleService> _logger;

        public TypeVehicleService(ITypeVehicleRepository typeVehicleRepository, ILogger<TypeVehicleService> logger)
        {
            _typeVehicleRepository = typeVehicleRepository ?? throw new ArgumentNullException(nameof(typeVehicleRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<TypeVehicle>> GetAllTypeVehiclesAsync()
        {
            return await _typeVehicleRepository.GetAllAsync();
        }

        public async Task<TypeVehicle?> GetTypeVehicleByIdAsync(int id)
        {
            return await _typeVehicleRepository.GetByIdAsync(id);
        }

        public async Task<TypeVehicle> CreateTypeVehicleAsync(TypeVehicle typeVehicle)
        {
            // Aquí podrías agregar lógica de negocio antes de crear el tipo de vehículo
            return await _typeVehicleRepository.AddAsync(typeVehicle);
        }

        public async Task<bool> UpdateTypeVehicleAsync(TypeVehicle typeVehicle)
        {
            // Aquí podrías agregar lógica de negocio antes de actualizar el tipo de vehículo
            return await _typeVehicleRepository.UpdateAsync(typeVehicle);
        }

        public async Task<bool> DeleteTypeVehicleAsync(int id)
        {
            // Aquí podrías agregar lógica de negocio antes de eliminar el tipo de vehículo
            return await _typeVehicleRepository.DeleteAsync(id);
        }
    }
}
