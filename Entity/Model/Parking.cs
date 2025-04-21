using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class Parking
    {
        public int id { get; set; }
        public string name { get; set; }
        public string hability { get; set; }
        public string location { get; set; }

        // Relación uno a muchos con Camara
        public int id_camara { get; set; }            // FK
        public Camara camara { get; set; }

        // Relación con VehicleHistoryParkingRates
        public List<VehicleHistoryParkingRates> VehicleHistoryParkingRates { get; set; } = new List<VehicleHistoryParkingRates>();
    }
}
