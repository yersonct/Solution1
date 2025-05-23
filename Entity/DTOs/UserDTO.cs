using System;
using System.Collections.Generic;

namespace Entity.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        // No incluir Password en DTOs de lectura por seguridad
        public int PersonId { get; set; }
        public string PersonName { get; set; } // Para mostrar el nombre de la persona
        public bool Active { get; set; }
    }
}