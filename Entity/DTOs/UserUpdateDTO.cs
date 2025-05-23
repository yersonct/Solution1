using System.ComponentModel.DataAnnotations;

namespace Entity.DTOs
{
    // Usaremos un DTO específico para la actualización para mayor claridad y control.
    // Podría ser idéntico a UserCreateDTO o tener campos opcionales/diferentes.
    public class UserUpdateDTO
    {
        // En PUT, el ID generalmente viene en la ruta, no en el cuerpo del DTO.
        // Pero si decides enviarlo en el cuerpo para validación extra, lo incluyes.
        // [Required]
        // public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre de usuario debe tener entre 3 y 50 caracteres.")]
        public string UserName { get; set; }

        // La contraseña puede ser opcional para actualizar (si no se envía, no se cambia)
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; } // 'string?' indica que es anulable/opcional

        [Required(ErrorMessage = "El ID de la persona es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID de la persona debe ser un número positivo.")]
        public int PersonId { get; set; }

        public bool Active { get; set; }
    }
}