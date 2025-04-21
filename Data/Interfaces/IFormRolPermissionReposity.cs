using Entity.DTOs;
using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IFormRolPermissionRepository
    {
        Task<IEnumerable<FormRolPermissionDTO>> GetAllAsync();
        Task<FormRolPermissionDTO?> GetByIdAsync(int id);
        Task<FormRolPermissionDTO> AddAsync(FormRolPermissionCreateDTO entity);
        Task<bool> UpdateAsync(FormRolPermissionDTO entity);
        Task<bool> DeleteAsync(int id);
    }
}
