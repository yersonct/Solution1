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
    public class RatesService : IRatesService
    {
        private readonly IRatesRepository _ratesRepository;
        private readonly ILogger<RatesService> _logger;

        public RatesService(IRatesRepository ratesRepository, ILogger<RatesService> logger)
        {
            _ratesRepository = ratesRepository ?? throw new ArgumentNullException(nameof(ratesRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Rates>> GetAllRatesAsync()
        {
            return await _ratesRepository.GetAllAsync();
        }

        public async Task<Rates?> GetRatesByIdAsync(int id)
        {
            return await _ratesRepository.GetByIdAsync(id);
        }

        public async Task<Rates> CreateRatesAsync(Rates rates)
        {
            // Aquí podrías agregar lógica de negocio antes de crear la tarifa
            return await _ratesRepository.AddAsync(rates);
        }

        public async Task<bool> UpdateRatesAsync(Rates rates)
        {
            // Aquí podrías agregar lógica de negocio antes de actualizar la tarifa
            return await _ratesRepository.UpdateAsync(rates);
        }

        public async Task<bool> DeleteRatesAsync(int id)
        {
            // Aquí podrías agregar lógica de negocio antes de eliminar la tarifa
            return await _ratesRepository.DeleteAsync(id);
        }
    }
}
