using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IPersonService
    {
        Task<IEnumerable<Person>> GetAllPersonsAsync();
        Task<Person?> GetPersonByIdAsync(int id);
        Task<Person> CreatePersonAsync(Person person);
        Task<bool> UpdatePersonAsync(Person person);
        Task<bool> DeletePersonAsync(int id);
    }
}
