using System.ComponentModel.DataAnnotations;

namespace Entity.DTOs
{
    public class FormModuleCreateDTO
    {
        [Required(ErrorMessage = "El ID del formulario es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del formulario debe ser un número positivo.")]
        public int FormId { get; set; } // Cambiado de id_forms a FormId

        [Required(ErrorMessage = "El ID del módulo es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del módulo debe ser un número positivo.")]
        public int ModuleId { get; set; } // Cambiado de id_module a ModuleId

        public bool Active { get; set; } = true;
    }
}