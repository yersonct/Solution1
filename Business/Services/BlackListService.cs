using Business.Interfaces;
using Data.Interfaces;
using Entity.DTOs;
using Entity.Model;
using Repository;
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
            var data = await _repo.GetAllAsync();
            return data.Select(b => new BlackListDTO
            {
                id = b.id,
                reason = b.reason,
                restrictiondate = b.restrictiondate,
                id_client = b.id_client
            });
        }

        public async Task<BlackListDTO?> GetByIdAsync(int id)
        {
            var b = await _repo.GetByIdAsync(id);
            if (b == null) return null;

            return new BlackListDTO
            {
                id = b.id,
                reason = b.reason,
                restrictiondate = b.restrictiondate,
                id_client = b.id_client
            };
        }
        public async Task CreateAsync(BlackListDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.reason))
                throw new ValidationException("La razón es obligatoria.");

            var entity = new BlackList
            {
                reason = dto.reason,
                restrictiondate = dto.restrictiondate,
                id_client = dto.id_client
            };

            await _repo.AddAsync(entity); // O llama tu método directo en la capa Data
        }

        //public async Task<IEnumerable<BlackListDTO>> GetAllWithClientAsync()
        //{
        //    var data = await _repo.GetAllWithClientAsync();
        //    return data.Select(b => new BlackListDTO
        //    {
        //        id = b.id,
        //        reason = b.reason,
        //        restrictiondate = b.restrictiondate,
        //        id_client = b.id_client,
        //        // Puedes extender aquí si agregaste campos del cliente en el DTO
        //    });
        //}

        //public async Task<BlackListDTO?> GetByIdWithClientAsync(int id)
        //{
        //    var b = await _repo.GetByIdWithClientAsync(id);
        //    if (b == null) return null;

        //    return new BlackListDTO
        //    {
        //        id = b.id,
        //        reason = b.reason,
        //        restrictiondate = b.restrictiondate,
        //        id_client = b.id_client,
        //        // Puedes extender aquí si agregaste campos del cliente en el DTO
        //    };
        //}

        public async Task UpdateAsync(BlackListDTO dto)
        {
            var entity = new BlackList
            {
                id = dto.id,
                reason = dto.reason,
                restrictiondate = dto.restrictiondate,
                id_client = dto.id_client
            };
            _repo.Update(entity);
            await _repo.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity != null)
            {
                _repo.Delete(entity);
                await _repo.SaveChangesAsync();
            }
        }
    }
}
