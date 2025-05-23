// Business/Interfaces/IUserService.cs

using Entity.DTOs; // Usar DTOs para la interfaz pública
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IUserService
    {
        // Métodos de lectura que devuelven DTOs con el nombre de la persona
        Task<IEnumerable<UserDTO>> GetAllUsersWithPersonNameAsync();
        Task<UserDTO?> GetUserWithPersonNameByIdAsync(int id);

        // Métodos de escritura que reciben DTOs y devuelven el DTO creado/actualizado
        Task<UserDTO> CreateUserAsync(UserCreateDTO userCreateDto);
        Task<bool> UpdateUserAsync(int id, UserUpdateDTO userUpdateDto); // Recibe el ID y el DTO de actualización
        Task<bool> DeleteUserAsync(int id);
    }
}