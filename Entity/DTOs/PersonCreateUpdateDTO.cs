using System.ComponentModel.DataAnnotations;

namespace Entity.DTOs
{
    public class PersonCreateUpdateDTO
    {
        // El Id no se incluye aquí, ya que se genera en la creación y se pasa por la ruta en la actualización.

        [Required(ErrorMessage = "El nombre es requerido.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El apellido es requerido.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 100 caracteres.")]
        public string LastName { get; set; }

        [StringLength(20, ErrorMessage = "El documento no puede exceder los 20 caracteres.")]
        // [Required(ErrorMessage = "El documento es requerido.")] // Quita el comentario si siempre es requerido
        public string Document { get; set; }

        [StringLength(20, ErrorMessage = "El teléfono no puede exceder los 20 caracteres.")]
        public string Phone { get; set; }

        [StringLength(100, ErrorMessage = "El email no puede exceder los 100 caracteres.")]
        [EmailAddress(ErrorMessage = "Formato de email inválido.")]
        public string Email { get; set; }

        public bool Active { get; set; } = true; // Por defecto activo al crear/actualizar
    }
}