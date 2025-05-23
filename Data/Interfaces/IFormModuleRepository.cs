// Data/Interfaces/IFormModuleRepository.cs

using Entity.DTOs; // Esto ya no es necesario aquí si el repositorio solo maneja entidades
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
        // Los tipos de retorno y parámetros deben ser FormModule (la entidad)
        Task<IEnumerable<FormModule>> GetAllAsync(); // Devuelve entidades
        Task<FormModule?> GetByIdAsync(int id); // Devuelve una entidad
        Task<FormModule> AddAsync(FormModule entity); // Recibe una entidad
        Task<bool> UpdateAsync(FormModule entity); // Recibe una entidad para actualizar
        Task<bool> DeleteAsync(int id);
    }
}