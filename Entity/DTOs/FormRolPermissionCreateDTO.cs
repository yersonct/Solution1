using System.ComponentModel.DataAnnotations;

namespace Entity.DTOs
{
    public class FormRolPermissionCreateDTO
    {
        [Required(ErrorMessage = "El ID del formulario es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del formulario debe ser un número positivo.")]
        public int FormId { get; set; } // Cambiado de id_forms a FormId

        [Required(ErrorMessage = "El ID del rol es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del rol debe ser un número positivo.")]
        public int RolId { get; set; } // Cambiado de id_rol a RolId

        [Required(ErrorMessage = "El ID del permiso es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del permiso debe ser un número positivo.")]
        public int PermissionId { get; set; } // Cambiado de id_permission a PermissionId

        public bool Active { get; set; } = true;
    }
}