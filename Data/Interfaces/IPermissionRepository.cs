using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IPermissionRepository
    {
        Task<Permission> AddAsync(Permission entity);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Permission>> GetAllAsync();
        Task<Permission?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(Permission entity);
    }
}
