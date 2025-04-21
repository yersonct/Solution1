using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class FormModuleDTO
    {
        public int Id { get; set; }
        public int id_forms { get; set; }
        public string FormName { get; set; }
        public int id_module { get; set; }
        public string ModuleName { get; set; }

    }
}
