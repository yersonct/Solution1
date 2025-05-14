using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Validators;

namespace ANPRVisionAPI.Extensions
{
    public static class FluentValidationCollectionExtensions
    {
        public static IServiceCollection AddFluentValidationConfiguration(this IServiceCollection services)
        {
            services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RegisterRequestValidator>());
            return services;
        }
    }
}