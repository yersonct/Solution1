using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class TypeRates
    {
        public int id { get; set; }
        public string name { get; set; }
        public double price { get; set; }

        // navegación inversa opcional:
        public ICollection<Rates> rates { get; set; }
    }
}
