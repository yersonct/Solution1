using Business.Interfaces;
using Data.Interfaces;
using Entity.DTOs;
using Entity.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Exceptions;

namespace Business.Services
{
    public class BlackListService : IBlackListService
    {
        private readonly IBlacklistRepository _repo;

        public BlackListService(IBlacklistRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<BlackListDTO>> GetAllAsync()
        {
            var data = await _repo.GetAllWithClientAsync();
            return data.Select(b => new BlackListDTO
            {
                id = b.id,
                reason = b.reason,
                restrictiondate = b.restrictiondate,
                id_client = b.id_client,
                clientName = b.client?.name,
                active = b.active
            });
        }

        public async Task<BlackListDTO?> GetByIdAsync(int id)
        {
            var b = await _repo.GetByIdWithClientAsync(id);
            if (b == null) return null;

            return new BlackListDTO
            {
                id = b.id,
                reason = b.reason,
                restrictiondate = b.restrictiondate,
                id_client = b.id_client,
                clientName = b.client?.name,
                active = b.active
            };
        }

        public async Task CreateAsync(BlackListDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.reason))
                throw new ValidationException("La razón es obligatoria.");

            var entity = new BlackList
            {
                reason = dto.reason,
                restrictiondate = dto.restrictiondate.ToUniversalTime(), // <--- Convertir a UTC al crear
                id_client = dto.id_client
            };

            await _repo.AddBlacklistAsync(entity);
        }

        public async Task UpdateAsync(BlackListDTO dto)
        {
            var entity = await _repo.GetByIdAsync(dto.id);
            if (entity == null || !entity.active) throw new FileNotFoundException("Entrada de la lista negra no encontrada o inactiva.");

            entity.reason = dto.reason;
            entity.restrictiondate = dto.restrictiondate.ToUniversalTime(); // <--- Convertir a UTC al actualizar
            entity.id_client = dto.id_client;

            _repo.Update(entity);
            await _repo.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null || !entity.active) throw new FileNotFoundException("Entrada de la lista negra no encontrada o ya inactiva.");

            await _repo.DeleteAsync(entity); // La conversión a UTC debería ocurrir antes si la entidad ya existe
        }
    }
}