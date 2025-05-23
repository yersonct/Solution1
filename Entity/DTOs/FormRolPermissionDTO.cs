using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class FormRolPermissionDTO
    {
        public int Id { get; set; }
        public string FormName { get; set; }
        public string RolName { get; set; }
        public string PermissionName { get; set; }
        public bool Active { get; set; }
    }
}