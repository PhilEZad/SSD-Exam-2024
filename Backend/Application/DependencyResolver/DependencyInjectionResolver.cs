using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyResolver;

public static class DependencyInjectionResolver
{
    public static void RegisterApplicationLayer(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<INoteService, NoteService>();
        serviceCollection.AddScoped<IAuthService, AuthService>();
    }
}