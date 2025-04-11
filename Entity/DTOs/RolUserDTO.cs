using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class RolUserDTO
    {
        public int Id { get; set; }
        public int RolId { get; set; }

        public int UserId { get; set; }
        public String RolName { get; set; }
        public String UserName { get; set; }
    }
}
