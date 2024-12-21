using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Security.DependencyResolver;

public static class DependencyInjectionResolver
{
    public static void RegisterSecurity(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtProvider, JwtProvider>();
    }
}