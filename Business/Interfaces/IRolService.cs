// Business/Interfaces/IRolService.cs

using Entity.DTOs; // Usar DTOs para la interfaz pública
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IRolService
    {
        // Métodos de lectura que devuelven DTOs
        Task<IEnumerable<RolDTO>> GetAllRolesAsync();
        Task<RolDTO?> GetRolByIdAsync(int id);

        // Métodos de escritura que reciben DTOs y devuelven DTOs o booleanos
        Task<RolDTO> CreateRolAsync(RolCreateUpdateDTO rolCreateDto);
        Task<bool> UpdateRolAsync(int id, RolCreateUpdateDTO rolUpdateDto); // Recibe el ID y el DTO de actualización
        Task<bool> DeleteRolAsync(int id); // Para borrado lógico
    }
}