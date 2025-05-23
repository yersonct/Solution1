// Business/Interfaces/IFormService.cs

using Entity.DTOs; // Usar DTOs para la interfaz pública
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IFormService
    {
        // Métodos de lectura que devuelven DTOs
        Task<IEnumerable<FormDTO>> GetAllFormsAsync();
        Task<FormDTO?> GetFormByIdAsync(int id);

        // Métodos de escritura que reciben DTOs y devuelven DTOs o booleanos
        Task<FormDTO> CreateFormAsync(FormCreateUpdateDTO formCreateDto);
        Task<bool> UpdateFormAsync(int id, FormCreateUpdateDTO formUpdateDto);
        Task<bool> DeleteFormAsync(int id);
    }
}