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
        Task<IEnumerable<RolUser>> GetAllRolUsersAsync();
        Task<RolUser?> GetRolUserByIdAsync(int id);
        Task<RolUser> CreateRolUserAsync(RolUser rolUser);
        Task<bool> UpdateRolUserAsync(RolUser rolUser);
        Task<bool> DeleteRolUserAsync(int id);
    }
}
