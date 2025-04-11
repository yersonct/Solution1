using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class VehicleHistory
    {
        public int id { get; set; }
        public TimeSpan totaltime { get; set; } // CAMBIO: DateTime a TimeSpan

        public int id_registeredvehicle { get; set; }
        public RegisteredVehicle registeredvehicle { get; set; }

        public int id_typevehicle { get; set; }
        public TypeVehicle typevehicle { get; set; }

        //public int? id_invoice { get; set; } // CAMBIO: int a int? para permitir nulos si es necesario
        //public Invoice Invoice { get; set; }
        public List<VehicleHistoryParkingRates> VehicleHistoryParkingRates { get; set; } = new List<VehicleHistoryParkingRates>();
    }
}