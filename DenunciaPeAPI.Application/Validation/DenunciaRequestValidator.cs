using System.ComponentModel.DataAnnotations;
using DenunciaPeAPI.Application.DTOs;

namespace DenunciaPeAPI.Application.Validation;

// Valida la estructura, formato y reglas de negocio de una solicitud de denuncia
// antes de que sea persistida, segun las relaciones obligatorias del modelo relacional.
public static class DenunciaRequestValidator
{
    public static IReadOnlyList<string> Validate(CreateDenunciaRequestDto request)
    {
        var errores = new List<string>();

        ValidateDataAnnotations(request, errores);

        if (request.Denunciado is not null
            && string.IsNullOrWhiteSpace(request.Denunciado.NombreApelativo)
            && string.IsNullOrWhiteSpace(request.Denunciado.Caracteristica))
        {
            errores.Add("El denunciado debe incluir al menos el nombre/apelativo o una caracteristica fisica.");
        }

        if (request.Evidencias is { Count: 0 })
        {
            errores.Add("Debe adjuntarse al menos una evidencia (nombre_archivo y url).");
        }

        if (request.FechaDenuncia != default && request.FechaDenuncia.Date > DateTime.UtcNow.Date)
        {
            errores.Add("La fecha de la denuncia no puede ser futura.");
        }

        if (request.Incidente is not null && request.Incidente.FechaIncidente != default
            && request.Incidente.FechaIncidente > DateTime.UtcNow)
        {
            errores.Add("La fecha del incidente no puede ser futura.");
        }

        return errores;
    }

    private static void ValidateDataAnnotations(CreateDenunciaRequestDto request, List<string> errores)
    {
        var context = new ValidationContext(request);
        var results = new List<ValidationResult>();
        if (!Validator.TryValidateObject(request, context, results, validateAllProperties: true))
        {
            errores.AddRange(results.Select(r => r.ErrorMessage ?? "Valor invalido."));
        }

        ValidateNested(request.Denunciante, "denunciante", errores);
        ValidateNested(request.Denunciado, "denunciado", errores);
        ValidateNested(request.Incidente, "incidente", errores);

        if (request.Evidencias is null)
        {
            return;
        }

        for (var i = 0; i < request.Evidencias.Count; i++)
        {
            ValidateNested(request.Evidencias[i], $"evidencia[{i}]", errores);
        }
    }

    private static void ValidateNested(object? instance, string entidad, List<string> errores)
    {
        if (instance is null)
        {
            errores.Add($"La relacion obligatoria '{entidad}' no fue enviada.");
            return;
        }

        var context = new ValidationContext(instance);
        var results = new List<ValidationResult>();
        if (!Validator.TryValidateObject(instance, context, results, validateAllProperties: true))
        {
            errores.AddRange(results.Select(r => $"{entidad}: {r.ErrorMessage}"));
        }
    }
}
