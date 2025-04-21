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
    public class ParkingService : IParkingService
    {
        private readonly IParkingRepository _parkingRepository;
        private readonly ILogger<ParkingService> _logger;

        public ParkingService(IParkingRepository parkingRepository, ILogger<ParkingService> logger)
        {
            _parkingRepository = parkingRepository ?? throw new ArgumentNullException(nameof(parkingRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Parking>> GetAllParkingsAsync()
        {
            return await _parkingRepository.GetAllAsync();
        }

        public async Task<Parking?> GetParkingByIdAsync(int id)
        {
            return await _parkingRepository.GetByIdAsync(id);
        }

        public async Task<Parking> CreateParkingAsync(Parking parking)
        {
            // Aquí podrías agregar lógica de negocio antes de crear el parking
            return await _parkingRepository.AddAsync(parking);
        }

        public async Task<bool> UpdateParkingAsync(Parking parking)
        {
            // Aquí podrías agregar lógica de negocio antes de actualizar el parking
            return await _parkingRepository.UpdateAsync(parking);
        }

        public async Task<bool> DeleteParkingAsync(int id)
        {
            // Aquí podrías agregar lógica de negocio antes de eliminar el parking
            return await _parkingRepository.DeleteAsync(id);
        }
    }
}
