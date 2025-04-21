using Entity.DTOs;
using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IFormModuleRepository
    {
        Task<IEnumerable<FormModuleDTO>> GetAllAsync();
        Task<FormModuleDTO?> GetByIdAsync(int id);
        Task<FormModuleDTO> AddAsync(FormModuleCreateDTO entity);
        Task<bool> UpdateAsync(int id, FormModuleCreateDTO entity);
        Task<bool> DeleteAsync(int id);
    }
}
