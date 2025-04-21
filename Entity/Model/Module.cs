using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class Modules
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<FormModule> FormModules { get; set; } = new List<FormModule>();
    }

}
