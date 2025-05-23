using System.ComponentModel.DataAnnotations;

namespace Entity.DTOs
{
    public class FormCreateUpdateDTO
    {
        [Required(ErrorMessage = "El nombre del formulario es requerido.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre del formulario debe tener entre 3 y 100 caracteres.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "La URL del formulario es requerida.")]
        [StringLength(255, ErrorMessage = "La URL no debe exceder los 255 caracteres.")]
        public string Url { get; set; }

        public bool Active { get; set; } = true; // Por defecto activo al crear/actualizar
    }
}