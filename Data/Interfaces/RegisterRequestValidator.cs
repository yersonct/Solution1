using Entity.DTOs;
using FluentValidation;

namespace Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(r => r.Person.Name)
                .NotEmpty().WithMessage("El Nombre es requerido.")
                .MaximumLength(50).WithMessage("El Nombre no puede tener más de 100 caracteres.");

            RuleFor(r => r.Person.LastName)
                .NotEmpty().WithMessage("El Apellido es requerido.")
                .MaximumLength(50).WithMessage("El Apellido no puede tener más de 100 caracteres.");

            RuleFor(r => r.Person.Document)
                .NotEmpty().WithMessage("El Documento es requerido.")
                .MaximumLength(20).WithMessage("El Documento no puede tener más de 20 caracteres.");

            RuleFor(r => r.Person.Email)
                .NotEmpty().WithMessage("El Email es requerido.")
                .EmailAddress().WithMessage("El Email no tiene un formato válido.")
                .MaximumLength(100).WithMessage("El Email no puede tener más de 100 caracteres.");

            RuleFor(r => r.Username)
                .NotEmpty().WithMessage("El Nombre de usuario es requerido.")
                .MinimumLength(3).WithMessage("El Nombre de usuario debe tener al menos 3 caracteres.")
                .MaximumLength(50).WithMessage("El Nombre de usuario no puede tener más de 50 caracteres.");

            RuleFor(r => r.Password)
                .NotEmpty().WithMessage("La Contraseña es requerida.")
                .MinimumLength(6).WithMessage("La Contraseña debe tener al menos 6 caracteres.")
                .MaximumLength(20).WithMessage("La Contraseña no puede tener más de 100 caracteres.");
            // Puedes agregar reglas más complejas para la contraseña si lo deseas
        }
    }
}