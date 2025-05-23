using System.ComponentModel.DataAnnotations;

namespace Entity.DTOs
{
    public class RolUserCreateDTO
    {
        // Id no suele ser parte de un DTO de creación
        // public int Id { get; set; }

        [Required(ErrorMessage = "El ID del rol es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del rol debe ser un número positivo.")]
        public int RolId { get; set; }

        [Required(ErrorMessage = "El ID del usuario es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del usuario debe ser un número positivo.")]
        public int UserId { get; set; }

        public bool Active { get; set; } = true;
    }
}