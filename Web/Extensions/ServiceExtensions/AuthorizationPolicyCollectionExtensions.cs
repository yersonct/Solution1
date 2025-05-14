using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace ANPRVisionAPI.Extensions
{
    public static class AuthorizationPolicyCollectionExtensions
    {
        public static IServiceCollection AddApplicationAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                // Políticas específicas para JWT
                options.AddPolicy("JwtAuthenticated", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(AuthenticationCollectionExtensions.JwtSchemeName)
                    .RequireAuthenticatedUser()
                    .Build());

                // Políticas específicas para OAuth 2.0
                options.AddPolicy("OAuthAuthenticated", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes("OAuthBearer")
                    .RequireAuthenticatedUser()
                    .Build());

                // Política que requiere cualquiera de los esquemas
                options.AddPolicy("AuthenticatedWithAny", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(AuthenticationCollectionExtensions.JwtSchemeName, "OAuthBearer")
                    .RequireAuthenticatedUser()
                    .Build());

                // Aquí puedes agregar más políticas basadas en roles, claims, etc.
            });

            return services;
        }
    }
}