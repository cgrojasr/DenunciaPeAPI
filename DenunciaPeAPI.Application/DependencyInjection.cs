using DenunciaPeAPI.Application.Interfaces;
using DenunciaPeAPI.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DenunciaPeAPI.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IDenunciaService, DenunciaService>();
        services.AddScoped<IAuditoriaService, AuditoriaService>();
        return services;
    }
}
