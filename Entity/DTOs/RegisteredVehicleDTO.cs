using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class RegisteredVehicleDTO
    {
        public int id { get; set; }
        public TimeSpan entrydatetime { get; set; } // CAMBIO: DateTime a TimeSpan
        public TimeSpan exitdatetime { get; set; }  // CAMBIO: DateTime a TimeSpan

        public int id_vehicle { get; set; }
        public Vehicle vehicleId { get; set; }

        public VehicleHistory vehiclehistory { get; set; }
    }
}