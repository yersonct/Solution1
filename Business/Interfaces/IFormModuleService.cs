using Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IFormModuleService
    {
        Task<IEnumerable<FormModuleDTO>> GetAllFormModulesAsync();
        Task<FormModuleDTO?> GetFormModuleByIdAsync(int id);
        Task<FormModuleDTO> CreateFormModuleAsync(FormModuleCreateDTO formModule);
        Task<bool> UpdateFormModuleAsync(int id, FormModuleCreateDTO formModule);
        Task<bool> DeleteFormModuleAsync(int id);
    }
}
