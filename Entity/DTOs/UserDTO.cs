﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Entity.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        //public bool Active { get; set; }
        public string Password { get; set; }
        public int PersonId { get; set; }
        public string PersonName { get; set; }

    }
}
