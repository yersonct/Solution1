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
    public class TypeRatesService : ITypeRatesService
    {
        private readonly ITypeRatesRepository _typeRatesRepository;
        private readonly ILogger<TypeRatesService> _logger;

        public TypeRatesService(ITypeRatesRepository typeRatesRepository, ILogger<TypeRatesService> logger)
        {
            _typeRatesRepository = typeRatesRepository ?? throw new ArgumentNullException(nameof(typeRatesRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<TypeRates>> GetAllTypeRatesAsync()
        {
            return await _typeRatesRepository.GetAllAsync();
        }

        public async Task<TypeRates?> GetTypeRatesByIdAsync(int id)
        {
            return await _typeRatesRepository.GetByIdAsync(id);
        }

        public async Task<TypeRates> CreateTypeRatesAsync(TypeRates typeRates)
        {
            // Aquí podrías agregar lógica de negocio antes de crear el tipo de tarifa
            return await _typeRatesRepository.AddAsync(typeRates);
        }

        public async Task<bool> UpdateTypeRatesAsync(TypeRates typeRates)
        {
            // Aquí podrías agregar lógica de negocio antes de actualizar el tipo de tarifa
            return await _typeRatesRepository.UpdateAsync(typeRates);
        }

        public async Task<bool> DeleteTypeRatesAsync(int id)
        {
            // Aquí podrías agregar lógica de negocio antes de eliminar el tipo de tarifa
            return await _typeRatesRepository.DeleteAsync(id);
        }
    }
}
