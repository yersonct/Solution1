using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class BlackList
    {
        public int id { get; set; }
        public string reason { get; set; }
        public DateTime restrictiondate { get; set; }

        // Esta propiedad actúa como clave foránea para la relación 1:1
        public int id_client { get; set; }
        public  Client Client { get; set; }
    }
}
