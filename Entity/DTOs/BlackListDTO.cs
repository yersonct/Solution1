// Entity.DTOs/BlackListDTO.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class BlackListDTO
    {
        public int id { get; set; }
        public string reason { get; set; }
        public DateTime restrictiondate { get; set; }
        public int id_client { get; set; }

        public string clientName { get; set; } // Para mostrar el nombre del cliente
        public bool active { get; set; }
    }
}