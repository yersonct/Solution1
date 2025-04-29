using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "La información de la persona es requerida.")]
        public PersonDTO Person { get; set; }

        [Required(ErrorMessage = "El Nombre de usuario es requerido.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "La Contraseña es requerida.")]
        public string Password { get; set; }
    }
}
