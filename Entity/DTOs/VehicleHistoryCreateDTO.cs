using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class VehicleHistoryCreateDTO
    {
        public TimeSpan TotalTime { get; set; } // Debe ser TimeSpan para coincidir con la entidad
        public int RegisteredVehicleId { get; set; }
        public int TypeVehicleId { get; set; }
    }
}
