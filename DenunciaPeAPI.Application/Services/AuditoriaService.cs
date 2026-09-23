using DenunciaPeAPI.Application.Interfaces;
using DenunciaPeAPI.Domain.Entities;
using DenunciaPeAPI.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace DenunciaPeAPI.Application.Services;

public class AuditoriaService(IAuditoriaRepository repository, ILogger<AuditoriaService> logger) : IAuditoriaService
{
    private const string Entidad = "denuncia";

    public async Task RegistrarExitoAsync(string entidad, string detalle)
    {
        logger.LogInformation("Auditoria [{Entidad}] registrado correctamente: {Detalle}", entidad, detalle);
        await repository.CreateAsync(new AuditoriaEvento
        {
            Entidad = entidad,
            Evento = "REGISTRO_EXITOSO",
            Exitoso = true,
            Detalle = detalle
        });
    }

    public async Task RegistrarRechazoAsync(string entidad, IReadOnlyCollection<string> errores)
    {
        var detalle = string.Join(" | ", errores);
        logger.LogWarning("Auditoria [{Entidad}] solicitud rechazada: {Detalle}", entidad, detalle);
        await repository.CreateAsync(new AuditoriaEvento
        {
            Entidad = entidad,
            Evento = "SOLICITUD_RECHAZADA",
            Exitoso = false,
            Detalle = detalle
        });
    }
}
