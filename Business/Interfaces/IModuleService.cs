// Business/Interfaces/IModuleService.cs

using Entity.DTOs; // Usar DTOs para la interfaz pública
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IModuleService
    {
        // Métodos de lectura que devuelven DTOs
        Task<IEnumerable<ModuleDTO>> GetAllModulesAsync();
        Task<ModuleDTO?> GetModuleByIdAsync(int id);

        // Métodos de escritura que reciben DTOs y devuelven DTOs o booleanos
        Task<ModuleDTO> CreateModuleAsync(ModuleCreateUpdateDTO moduleCreateDto);
        Task<bool> UpdateModuleAsync(int id, ModuleCreateUpdateDTO moduleUpdateDto);
        Task<bool> DeleteModuleAsync(int id);
    }
}