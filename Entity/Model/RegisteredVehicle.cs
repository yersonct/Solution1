using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class RegisteredVehicle
    {
        public int id { get; set; }

        public int id_vehicle { get; set; }
        public TimeSpan entrydatetime { get; set; } // CAMBIO: DateTime a TimeSpan
        public TimeSpan exitdatetime { get; set; }  // CAMBIO: DateTime a TimeSpan


        public Vehicle vehicle { get; set; }

        public List<VehicleHistory> vehiclehistory { get; set; }

    }
}