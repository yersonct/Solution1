using System.ComponentModel.DataAnnotations;

namespace Entity.DTOs
{
    public class PermissionCreateUpdateDTO
    {
        // El Id no se incluye aquí, ya que se genera en la creación y se pasa por la ruta en la actualización.

        [Required(ErrorMessage = "El nombre del permiso es requerido.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre del permiso debe tener entre 3 y 50 caracteres.")]
        public string Name { get; set; }

        // Permitir que el cliente envíe el estado activo. Por defecto activo al crear/actualizar.
        public bool Active { get; set; } = true;
    }
}