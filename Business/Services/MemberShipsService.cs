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
    public class MembershipsService : IMembershipsService
    {
        private readonly IMembershipsRepository _membershipsRepository;
        private readonly ILogger<MembershipsService> _logger;

        public MembershipsService(IMembershipsRepository membershipsRepository, ILogger<MembershipsService> logger)
        {
            _membershipsRepository = membershipsRepository ?? throw new ArgumentNullException(nameof(membershipsRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<MemberShips>> GetAllMembershipsAsync()
        {
            return await _membershipsRepository.GetAllAsync();
        }

        public async Task<MemberShips?> GetMembershipByIdAsync(int id)
        {
            return await _membershipsRepository.GetByIdAsync(id);
        }

        public async Task<MemberShips> CreateMembershipAsync(MemberShips membership)
        {
            // Aquí podrías agregar lógica de negocio antes de crear la membresía
            return await _membershipsRepository.AddAsync(membership);
        }

        public async Task<bool> UpdateMembershipAsync(MemberShips membership)
        {
            // Aquí podrías agregar lógica de negocio antes de actualizar la membresía
            return await _membershipsRepository.UpdateAsync(membership);
        }

        public async Task<bool> DeleteMembershipAsync(int id)
        {
            // Aquí podrías agregar lógica de negocio antes de eliminar la membresía
            return await _membershipsRepository.DeleteAsync(id);
        }
    }
}
