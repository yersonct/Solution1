using System;
using Dapper;
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
    public class ClientBusiness
    {
        private readonly ClientData _clientData;
        private readonly ILogger<ClientBusiness> _logger;

        public ClientBusiness(ClientData clientData, ILogger<ClientBusiness> logger)
        {
            _clientData = clientData ?? throw new ArgumentNullException(nameof(clientData));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<ClientDTO>> GetAllAsync()
        {
            var clients = await _clientData.GetAllAsync();
            return clients.Select(c => new ClientDTO
            {
                Id = c.id,
                id_user = c.id_user,
                Name = c.name
            });
        }

        public async Task<ClientDTO> GetByIdAsync(int id)
        {
            var client = await _clientData.GetByIdAsync(id);
            if (client == null)
                throw new EntityNotFoundException($"Cliente con ID {id} no encontrado.");

            return new ClientDTO
            {
                Id = client.id,
                id_user = client.id_user,
                Name = client.name
            };
        }

        public async Task<ClientDTO> CreateAsync(ClientCreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.name))
                throw new ValidationException("El nombre del cliente no puede estar vacío.");

            var entity = new Client
            {
                id_user = dto.id_user,
                name = dto.name
            };

            var created = await _clientData.CreateAsync(entity);

            return new ClientDTO
            {
                Id = created.id,
                id_user = created.id_user,
                Name = created.name
            };
        }

        public async Task<bool> UpdateAsync(int id, ClientCreateDTO dto)
        {
            var client = await _clientData.GetByIdAsync(id);
            if (client == null)
                throw new EntityNotFoundException($"Cliente con ID {id} no encontrado.");

            if (string.IsNullOrWhiteSpace(dto.name))
                throw new ValidationException("El nombre del cliente no puede estar vacío.");

            client.id_user = dto.id_user;
            client.name = dto.name;

            return await _clientData.UpdateAsync(client);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var client = await _clientData.GetByIdAsync(id);
            if (client == null)
                throw new EntityNotFoundException($"Cliente con ID {id} no encontrado.");

            return await _clientData.DeleteAsync(id);
        }
    }
}
