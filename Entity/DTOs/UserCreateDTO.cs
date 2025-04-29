using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class UserCreateDTO
    {

        public int id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int PersonId { get; set; }
        //public int? ClientId { get; set; }  // Si aplica
        public bool active;
    }
}
