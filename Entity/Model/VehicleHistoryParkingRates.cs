using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class VehicleHistoryParkingRates
    {
        public int id { get; set; }
        public int id_vehiclehistory { get; set; }
        public  VehicleHistory vehiclehistory { get; set; }

        public int id_rates {  get; set; }
        public  Rates rates { get; set; }
        public int id_parking {  get; set; }
        public  Parking parking { get; set; }
        public int hourused { get; set; }
        public double subtotal {  get; set; }


    }
}
