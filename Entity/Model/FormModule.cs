using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class FormModule
    {
        public int id { get; set; }
        public int id_forms { get; set; }
        public int id_module { get; set; }

        public bool active {  get; set; }

        public Forms Forms { get; set; }
        public Modules Modules { get; set; }
    }
}
