using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class FormRolPermissionCreateDTO
    {
        public int id_forms { get; set; }
        public int id_role { get; set; }
        public int id_permission { get; set; }
    }
}
