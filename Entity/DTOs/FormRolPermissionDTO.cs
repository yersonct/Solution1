using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class FormRolPermissionDTO
    {
        public int id { get; set; }
        public string formName { get; set; }
        public string rolName { get; set; }
        public string permissionName { get; set; }
        public bool active { get; set; } // Add the active property
    }
}