using Entity.DTOs;
using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IRolUserService
    {
        Task<IEnumerable<RolUserDTO>> GetAllRolUsersAsync();
        Task<RolUserDTO?> GetRolUserByIdAsync(int id);
        Task<RolUserDTO> CreateRolUserAsync(RolUserCreateDTO rolUserDto); // Recibe DTO de creación
        Task<bool> UpdateRolUserAsync(int id, RolUserCreateDTO rolUserDto); // Recibe ID y DTO de actualización
        Task<bool> DeleteRolUserAsync(int id);
    }
}
