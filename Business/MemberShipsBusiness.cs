using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    public class MembershipsBusiness
    {
        private readonly MembershipsData _membershipsData;
        private readonly ILogger<MembershipsBusiness> _logger;

        public MembershipsBusiness(MembershipsData membershipsData, ILogger<MembershipsBusiness> logger)
        {
            _membershipsData = membershipsData ?? throw new ArgumentNullException(nameof(membershipsData));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<MembershipDTO>> GetAllMembershipsAsync()
        {
            try
            {
                var memberships = await _membershipsData.GetAllAsyncSQL();
                return MapToDtoList(memberships);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los membresías.");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de membresías.", ex);
            }
        }

        public async Task<MembershipDTO> GetMembershipByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ValidationException("id", "El ID debe ser mayor que cero.");
            }

            try
            {
                var membership = await _membershipsData.GetByIdAsyncSQL(id);
                if (membership == null)
                {
                    throw new EntityNotFoundException("Membership", id);
                }
                return MapToDTO(membership);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la membresía con ID: {MembershipId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la membresía con ID {id}.", ex);
            }
        }

        public async Task<MembershipDTO> CreateMembershipAsync(MembershipDTO membershipDTO)
        {
            try
            {
                ValidateMembership(membershipDTO);
                var membershipEntity = MapToEntity(membershipDTO);
                var createdMembership = await _membershipsData.CreateAsyncSQL(membershipEntity);
                return MapToDTO(createdMembership);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear una nueva membresía: {MembershipType}", membershipDTO?.membershiptype ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear la membresía.", ex);
            }
        }

        public async Task<bool> UpdateMembershipAsync(MembershipDTO membershipDTO)
        {
            try
            {
                ValidateMembership(membershipDTO);
                var existingMembership = await _membershipsData.GetByIdAsyncSQL(membershipDTO.id);
                if (existingMembership == null)
                {
                    throw new EntityNotFoundException("Membership", membershipDTO.id);
                }

                existingMembership.membershiptype = membershipDTO.membershiptype;
                existingMembership.startdate = membershipDTO.startdate;
                existingMembership.enddate = membershipDTO.enddate;
                existingMembership.active = membershipDTO.active;

                return await _membershipsData.UpdateAsyncSQL(existingMembership);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la membresía con ID: {MembershipId}", membershipDTO.id);
                throw new ExternalServiceException("Base de datos", "Error al actualizar la membresía.", ex);
            }
        }

        public async Task<bool> DeleteMembershipAsync(int id)
        {
            try
            {
                var existingMembership = await _membershipsData.GetByIdAsyncSQL(id);
                if (existingMembership == null)
                {
                    throw new EntityNotFoundException("Membership", id);
                }
                return await _membershipsData.DeleteAsyncSQL(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la membresía con ID: {MembershipId}", id);
                throw new ExternalServiceException("Base de datos", "Error al eliminar la membresía.", ex);
            }
        }

        private void ValidateMembership(MembershipDTO membershipDTO)
        {
            if (membershipDTO == null)
            {
                throw new ValidationException("El objeto Membership no puede ser nulo.");
            }
            if (string.IsNullOrWhiteSpace(membershipDTO.membershiptype))
            {
                throw new ValidationException("MembershipType", "El tipo de membresía es obligatorio.");
            }
        }

        private MembershipDTO MapToDTO(MemberShips membership)
        {
            return new MembershipDTO
            {
                id = membership.id,
                membershiptype = membership.membershiptype,
                startdate = membership.startdate,
                enddate = membership.enddate,
                active = membership.active
            };
        }

        private MemberShips MapToEntity(MembershipDTO membershipDTO)
        {
            return new MemberShips
            {
                id = membershipDTO.id,
                membershiptype = membershipDTO.membershiptype,
                startdate = membershipDTO.startdate,
                enddate = membershipDTO.enddate,
                active = membershipDTO.active
            };
        }

        private IEnumerable<MembershipDTO> MapToDtoList(IEnumerable<MemberShips> memberships)
        {
            var membershipsDto = new List<MembershipDTO>();
            foreach (var membership in memberships)
            {
                membershipsDto.Add(MapToDTO(membership));
            }
            return membershipsDto;
        }
    }
}
