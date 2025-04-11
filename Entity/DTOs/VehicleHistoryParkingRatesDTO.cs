using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class VehicleHistoryParkingRatesDTO
    {
        public int Id { get; set; }
        public int VehicleHistoryId { get; set; }
        public int RatesId { get; set; }
        public int ParkingId { get; set; }
        public int HoursUsed { get; set; }
        public double SubTotal { get; set; }
    }
}
