using Entity.DTOs;
using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IFormRolPermissionService
    {
        Task<IEnumerable<FormRolPermissionDTO>> GetAllFormRolPermissionsAsync();
        Task<FormRolPermissionDTO?> GetFormRolPermissionByIdAsync(int id);
        Task<FormRolPermissionDTO> CreateFormRolPermissionAsync(FormRolPermission formRolPermission);
        Task<bool> UpdateFormRolPermissionAsync(FormRolPermissionDTO formRolPermission);
        Task<bool> DeleteFormRolPermissionAsync(int id);
        
    }
}
