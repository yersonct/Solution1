using System.ComponentModel.DataAnnotations;

namespace Entity.DTOs
{
    public class RolCreateUpdateDTO
    {
        // No incluir 'Id' en DTOs de creación/actualización, ya que viene de la ruta para PUT y es generado por la BD para POST.
        // public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del rol es requerido.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre del rol debe tener entre 3 y 50 caracteres.")]
        public string Name { get; set; }

        [StringLength(255, ErrorMessage = "La descripción no puede exceder los 255 caracteres.")]
        public string Description { get; set; }

        // Permitir que el cliente envíe el estado activo, o establecer un valor por defecto en el DTO o servicio
        public bool Active { get; set; } = true; // Por defecto activo al crear/actualizar
    }
}
