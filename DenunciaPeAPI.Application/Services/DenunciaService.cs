using DenunciaPeAPI.Application.DTOs;
using DenunciaPeAPI.Application.Exceptions;
using DenunciaPeAPI.Application.Interfaces;
using DenunciaPeAPI.Application.Validation;
using DenunciaPeAPI.Domain.Entities;
using DenunciaPeAPI.Domain.Interfaces;

namespace DenunciaPeAPI.Application.Services;

public class DenunciaService(IDenunciaRepository repository, IAuditoriaService auditoriaService) : IDenunciaService
{
    private const string Entidad = "denuncia";

    public async Task<IEnumerable<DenunciaResumenDto>> GetAllAsync()
    {
        var denuncias = await repository.GetAllAsync();
        return denuncias.Select(d => new DenunciaResumenDto(d.Id, d.FechaDenuncia, d.IdTipoDenuncia));
    }

    public async Task<DenunciaDto?> GetByIdAsync(int id)
    {
        var denuncia = await repository.GetByIdAsync(id);
        return denuncia is null ? null : ToDto(denuncia);
    }

    public async Task<DenunciaDto> CreateAsync(CreateDenunciaRequestDto dto)
    {
        var errores = DenunciaRequestValidator.Validate(dto);
        if (errores.Count > 0)
        {
            await auditoriaService.RegistrarRechazoAsync(Entidad, errores);
            throw new DenunciaValidationException(errores);
        }

        var compuesta = new DenunciaCompuesta
        {
            Denuncia = new Denuncia { FechaDenuncia = dto.FechaDenuncia, IdTipoDenuncia = dto.IdTipoDenuncia },
            Denunciante = new Denunciante
            {
                TipoDocumento = dto.Denunciante.TipoDocumento,
                NroDocumento = dto.Denunciante.NroDocumento,
                Nombres = dto.Denunciante.Nombres,
                Apellidos = dto.Denunciante.Apellidos,
                NroTelefono = dto.Denunciante.NroTelefono,
                Direccion = dto.Denunciante.Direccion,
                Ubigeo = dto.Denunciante.Ubigeo
            },
            Denunciado = new Denunciado
            {
                NombreApelativo = dto.Denunciado.NombreApelativo,
                Caracteristica = dto.Denunciado.Caracteristica is null
                    ? null
                    : System.Text.Encoding.UTF8.GetBytes(dto.Denunciado.Caracteristica)
            },
            Incidente = new Incidente
            {
                FechaIncidente = dto.Incidente.FechaIncidente,
                UbigeoIncidente = dto.Incidente.UbigeoIncidente,
                DireccionIncidente = dto.Incidente.DireccionIncidente,
                Latitud = dto.Incidente.Latitud,
                Longitud = dto.Incidente.Longitud,
                Referencia = dto.Incidente.Referencia
            },
            Evidencias = dto.Evidencias
                .Select(e => new Evidencia { NombreArchivo = e.NombreArchivo, Url = e.Url })
                .ToList()
        };

        var id = await repository.CreateAsync(compuesta);
        await auditoriaService.RegistrarExitoAsync(Entidad, $"Denuncia registrada con id_denuncia={id}.");

        var creada = await repository.GetByIdAsync(id)
            ?? throw new InvalidOperationException("No se pudo recuperar la denuncia recien creada.");
        return ToDto(creada);
    }

    public Task<bool> DeleteAsync(int id) => repository.DeleteAsync(id);

    private static DenunciaDto ToDto(DenunciaCompuesta d) => new(
        d.Denuncia.Id,
        d.Denuncia.FechaDenuncia,
        d.Denuncia.IdTipoDenuncia,
        new DenuncianteDto(
            d.Denunciante.TipoDocumento,
            d.Denunciante.NroDocumento,
            d.Denunciante.Nombres,
            d.Denunciante.Apellidos,
            d.Denunciante.NroTelefono,
            d.Denunciante.Direccion,
            d.Denunciante.Ubigeo),
        new DenunciadoDto(
            d.Denunciado.NombreApelativo,
            d.Denunciado.Caracteristica is null ? null : System.Text.Encoding.UTF8.GetString(d.Denunciado.Caracteristica)),
        new IncidenteDto(
            d.Incidente.FechaIncidente,
            d.Incidente.UbigeoIncidente,
            d.Incidente.DireccionIncidente,
            d.Incidente.Latitud,
            d.Incidente.Longitud,
            d.Incidente.Referencia),
        d.Evidencias.Select(e => new EvidenciaDto(e.NombreArchivo, e.Url)).ToList());
}
