using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IRolService
    {
        Task<IEnumerable<Rol>> GetAllRolesAsync();
        Task<Rol?> GetRolByIdAsync(int id);
        Task<Rol> CreateRolAsync(Rol rol);
        Task<bool> UpdateRolAsync(Rol rol);
        Task<bool> DeleteRolAsync(int id);
    }
}
