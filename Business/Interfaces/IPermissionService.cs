// Business/Interfaces/IPermissionService.cs

using Entity.DTOs; // Usar DTOs para la interfaz pública
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IPermissionService
    {
        // Métodos de lectura que devuelven DTOs
        Task<IEnumerable<PermissionDTO>> GetAllPermissionsAsync();
        Task<PermissionDTO?> GetPermissionByIdAsync(int id);

        // Métodos de escritura que reciben DTOs y devuelven DTOs o booleanos
        Task<PermissionDTO> CreatePermissionAsync(PermissionCreateUpdateDTO permissionCreateDto);
        Task<bool> UpdatePermissionAsync(int id, PermissionCreateUpdateDTO permissionUpdateDto); // Recibe el ID y el DTO de actualización
        Task<bool> DeletePermissionAsync(int id); // Para borrado lógico
    }
}   