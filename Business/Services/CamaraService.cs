using Business.Interfaces;
using Data.Interfaces;
using Entity.DTOs;
using Entity.Model;

namespace Business.Services
{
    public class CamaraService : ICamaraService
    {
        private readonly ICamaraRepository _repository;

        public CamaraService(ICamaraRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CamaraDTO>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(e => MapToDTO(e));
        }

        public async Task<CamaraDTO?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? null : MapToDTO(entity);
        }

        public async Task<CamaraDTO> CreateAsync(CamaraDTO dto)
        {
            var entity = MapToEntity(dto);
            var created = await _repository.CreateAsync(entity);
            return MapToDTO(created);
        }

        public async Task<CamaraDTO?> UpdateAsync(int id, CamaraDTO dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return null;

            var updatedEntity = MapToEntity(dto);
            updatedEntity.id = id;

            var success = await _repository.UpdateAsync(updatedEntity);
            return success ? MapToDTO(updatedEntity) : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        private CamaraDTO MapToDTO(Camara camara)
        {
            return new CamaraDTO
            {
                id = camara.id,
                nightvisioninfrared = camara.nightvisioninfrared,
                highresolution = camara.highresolution,
                infraredlighting = camara.infraredlighting,
                name = camara.name,
                optimizedangleofvision = camara.optimizedangleofvision,
                highshutterspeed = camara.highshutterspeed
            };
        }

        private Camara MapToEntity(CamaraDTO dto)
        {
            return new Camara
            {
                id = dto.id,
                nightvisioninfrared = dto.nightvisioninfrared,
                highresolution = dto.highresolution,
                infraredlighting = dto.infraredlighting,
                name = dto.name,
                optimizedangleofvision = dto.optimizedangleofvision,
                highshutterspeed = dto.highshutterspeed
            };
        }
    }
}
