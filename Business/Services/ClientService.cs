using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Model;

namespace Business.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly ILogger<ClientService> _logger;

        public ClientService(IClientRepository clientRepository, ILogger<ClientService> logger)
        {
            _clientRepository = clientRepository ?? throw new ArgumentNullException(nameof(clientRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            return await _clientRepository.GetAllAsync();
        }

        public async Task<Client?> GetClientByIdAsync(int id)
        {
            return await _clientRepository.GetByIdAsync(id);
        }

        public async Task<Client> CreateClientAsync(Client client)
        {
            // Aquí podrías agregar lógica de negocio antes de crear el cliente
            return await _clientRepository.AddAsync(client);
        }

        public async Task<bool> UpdateClientAsync(Client client)
        {
            // Aquí podrías agregar lógica de negocio antes de actualizar el cliente
            return await _clientRepository.UpdateAsync(client);
        }

        public async Task<bool> DeleteClientAsync(int id)
        {
            // Aquí podrías agregar lógica de negocio antes de eliminar el cliente
            return await _clientRepository.DeleteAsync(id);
        }
    }
}