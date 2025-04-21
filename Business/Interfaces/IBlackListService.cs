using Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IBlackListService
    {
        Task<IEnumerable<BlackListDTO>> GetAllAsync();
        Task<BlackListDTO?> GetByIdAsync(int id);
        Task CreateAsync(BlackListDTO dto); // 👈 Nuevo método POST
        Task UpdateAsync(BlackListDTO dto);
        Task DeleteAsync(int id);
    }
}
