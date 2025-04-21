using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IParkingService
    {
        Task<IEnumerable<Parking>> GetAllParkingsAsync();
        Task<Parking?> GetParkingByIdAsync(int id);
        Task<Parking> CreateParkingAsync(Parking parking);
        Task<bool> UpdateParkingAsync(Parking parking);
        Task<bool> DeleteParkingAsync(int id);
    }
}
