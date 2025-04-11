using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class Rates
    {
        public int id { get; set; }
        public double amount { get; set; }
        public TimeSpan startduration { get; set; }
        public TimeSpan endduration { get; set; }

        public int id_typerates { get; set; } // ← agrega este campo
        public TypeRates TypeRates { get; set; } // Propiedad de navegación

        public List<VehicleHistoryParkingRates> VehicleHistoryParkingRates { get; set; } = new List<VehicleHistoryParkingRates>();
    }
}

