using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IPermissionService
    {
        Task<IEnumerable<Permission>> GetAllPermissionsAsync();
        Task<Permission?> GetPermissionByIdAsync(int id);
        Task<Permission> CreatePermissionAsync(Permission permission);
        Task<bool> UpdatePermissionAsync(Permission permission);
        Task<bool> DeletePermissionAsync(int id);
    }
}
