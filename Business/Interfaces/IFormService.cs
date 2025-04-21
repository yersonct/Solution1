using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IFormService
    {
        Task<IEnumerable<Forms>> GetAllFormsAsync();
        Task<Forms?> GetFormByIdAsync(int id);
        Task<Forms> CreateFormAsync(Forms form);
        Task<bool> UpdateFormAsync(Forms form);
        Task<bool> DeleteFormAsync(int id);
    }
}
