// Business/Interfaces/IPersonService.cs

using Entity.DTOs; // Usar DTOs para la interfaz pública
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IPersonService
    {
        // Métodos de lectura que devuelven DTOs
        Task<IEnumerable<PersonDTO>> GetAllPersonsAsync();
        Task<PersonDTO?> GetPersonByIdAsync(int id);

        // Métodos de escritura que reciben DTOs y devuelven DTOs o booleanos
        Task<PersonDTO> CreatePersonAsync(PersonCreateUpdateDTO personCreateDto);
        Task<bool> UpdatePersonAsync(int id, PersonCreateUpdateDTO personUpdateDto); // Recibe el ID y el DTO de actualización
        Task<bool> DeletePersonAsync(int id); // Para borrado lógico
    }
}