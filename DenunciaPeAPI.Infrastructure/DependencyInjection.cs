using DenunciaPeAPI.Domain.Interfaces;
using DenunciaPeAPI.Infrastructure.Data;
using DenunciaPeAPI.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DenunciaPeAPI.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<IDenunciaRepository, DenunciaRepository>();
        services.AddScoped<IAuditoriaRepository, AuditoriaRepository>();
        return services;
    }
}
