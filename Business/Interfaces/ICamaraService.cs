using Entity.DTOs;
using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ICamaraService
    {
        Task<IEnumerable<CamaraDTO>> GetAllAsync();
        Task<CamaraDTO> GetByIdAsync(int id);
        Task<CamaraDTO> CreateAsync(CamaraDTO dto);  // Usa el mismo DTO para crear
        Task<CamaraDTO> UpdateAsync(int id, CamaraDTO dto); // Igual acá
        Task<bool> DeleteAsync(int id);
    }
}
