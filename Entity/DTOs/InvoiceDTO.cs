using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class InvoiceDTO
    {
        public int id { get; set; }
        public decimal totalamount { get; set; }
        public string paymentstatus { get; set; }
        public DateTime? paymentdate { get; set; }
        public int vehiclehistoryid { get; set; }
    }

}
