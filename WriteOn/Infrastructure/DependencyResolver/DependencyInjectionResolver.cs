using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyResolver;

public static class DependencyInjectionResolver
{
    public static void RegisterInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<INoteRepository, NoteRepository>();
    }
}