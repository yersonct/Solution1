using Entity.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IPersonRepository
    {
        Task<Person> AddAsync(Person entity);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Person>> GetAllAsync();
        Task<Person?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(Person entity);
        Task<bool> PersonExistsAsync(string document, string email);
    }
}