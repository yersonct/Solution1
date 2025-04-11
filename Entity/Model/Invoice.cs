using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class Invoice
    {
        public int id { get; set; }
        public decimal totalamount { get; set; }
        public string paymentstatus { get; set; }
        public DateTime? paymentdate { get; set; } // CAMBIO: TimeSpan a DateTime

        public int id_vehiclehistory { get; set; } // Agregar la propiedad de llave foránea explícita
        public VehicleHistory vehiclehistory { get; set; } // solo navegación
    }
}