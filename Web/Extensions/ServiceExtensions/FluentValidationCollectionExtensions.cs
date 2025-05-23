// Web/Extensions/ServiceExtensions/FluentValidationCollectionExtensions.cs

using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Business.Validations; // Nuevo: Asegúrate de que este namespace sea correcto para UserCreateDTOValidator
using Validators; // Mantén este using para tu RegisterRequestValidator si está en un ensamblado diferente o si quieres mantenerlo explícito.
                  // Si todos tus validadores están en 'Business.Validations', puedes remover 'using Validators;'.

namespace ANPRVisionAPI.Extensions
{
    public static class FluentValidationCollectionExtensions
    {
        public static IServiceCollection AddFluentValidationConfiguration(this IServiceCollection services)
        {
            // Registra automáticamente todos los validadores que hereden de AbstractValidator
            // en el ensamblado donde se encuentra UserCreateDTOValidator.
            // Si tus validadores 'RegisterRequestValidator' y 'UserCreateDTOValidator' están en el mismo proyecto/ensamblado
            // (por ejemplo, ambos en el proyecto Business, o ambos en Web si tu proyecto Web los contiene),
            // con un solo 'RegisterValidatorsFromAssemblyContaining' que apunte a una clase dentro de ese ensamblado es suficiente.
            services.AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<UserCreateDTOValidator>();
                // Si tienes validadores en otros ensamblados, necesitarías otra línea similar:
                // fv.RegisterValidatorsFromAssemblyContaining<SomeOtherValidatorInDifferentAssembly>();

                // Opcional: Deshabilitar la validación de DataAnnotations si solo usarás FluentValidation
                fv.DisableDataAnnotationsValidation = true;
            });

            return services;
        }
    }
}