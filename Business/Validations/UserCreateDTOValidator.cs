using Data.Interfaces;
using Entity.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validations
{
    public class UserCreateDTOValidator : AbstractValidator<UserCreateDTO>
    {
        private readonly IUserRepository _userRepository; // Inyectaremos este repositorio para validaciones asíncronas

        public UserCreateDTOValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            // Regla para UserName: No vacío, longitud, y unicidad
            RuleFor(dto => dto.UserName)
                .NotEmpty().WithMessage("El nombre de usuario es requerido.")
                .Length(3, 50).WithMessage("El nombre de usuario debe tener entre 3 y 50 caracteres.")
                .MustAsync(async (userName, cancellation) =>
                {
                    // Verifica si el nombre de usuario ya existe en la base de datos
                    return !(await _userRepository.UsernameExistsAsync(userName));
                })
                .WithMessage("El nombre de usuario ya está en uso. Por favor, elige otro.");

            // Regla para Password: No vacío, longitud mínima y complejidad
            RuleFor(dto => dto.Password)
                .NotEmpty().WithMessage("La contraseña es requerida.")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.")
                .Matches("[A-Z]").WithMessage("La contraseña debe contener al menos una letra mayúscula.")
                .Matches("[a-z]").WithMessage("La contraseña debe contener al menos una letra minúscula.")
                .Matches("[0-9]").WithMessage("La contraseña debe contener al menos un número.")
                .Matches("[^a-zA-Z0-9]").WithMessage("La contraseña debe contener al menos un carácter especial.");

            // Regla para PersonId: Requerido y existencia en la base de datos
            RuleFor(dto => dto.PersonId)
                .GreaterThan(0).WithMessage("El ID de la persona es requerido y debe ser un número positivo.")
                .MustAsync(async (personId, cancellation) =>
                {
                    // Verifica si la PersonId existe en la base de datos
                    return await _userRepository.PersonExistsAsync(personId);
                })
                .WithMessage("La persona especificada no existe.");

            // Regla para Active: (Opcional, si siempre debe ser true o si tiene lógica específica)
            // RuleFor(dto => dto.Active)
            //     .Equal(true).WithMessage("El usuario debe estar activo por defecto al crear.");
        }
    }
}
