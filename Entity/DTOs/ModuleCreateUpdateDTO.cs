using System.ComponentModel.DataAnnotations;

namespace Entity.DTOs
{
    public class ModuleCreateUpdateDTO
    {
        [Required(ErrorMessage = "El nombre del módulo es requerido.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre del módulo debe tener entre 3 y 100 caracteres.")]
        public string Name { get; set; }

        public bool Active { get; set; } = true; // Por defecto activo al crear/actualizar
    }
}