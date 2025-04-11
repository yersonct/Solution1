using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class TypeRates
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        // navegación inversa opcional:
        public ICollection<Rates> rates { get; set; }
    }
}
