﻿using System.ComponentModel.DataAnnotations;

namespace Entity.DTOs
{
    public class UserCreateDTO
    {
        // Id no suele ser parte de un DTO de creación, ya que lo genera la BD
        // public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre de usuario debe tener entre 3 y 50 caracteres.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida.")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        [DataType(DataType.Password)] // Sugerencia para UI, no validación per se
        public string Password { get; set; }

        [Required(ErrorMessage = "El ID de la persona es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID de la persona debe ser un número positivo.")]
        public int PersonId { get; set; }

        // public int? ClientId { get; set; } // Si aplica

        public bool Active { get; set; } = true; // Por defecto activo al crear
    }
}