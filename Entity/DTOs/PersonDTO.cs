using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class PersonDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Document { get; set; }
        public string Phone { get; set; }
        //public int Age { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
    }
}
