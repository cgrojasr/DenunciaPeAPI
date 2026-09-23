using System.ComponentModel.DataAnnotations;

namespace DenunciaPeAPI.Application.DTOs;

// Longitudes alineadas a las columnas varchar definidas en el script de base de datos.
public record DenuncianteDto(
    [Required, StringLength(10)] string TipoDocumento,
    [Required, StringLength(20)] string NroDocumento,
    [Required, StringLength(200)] string Nombres,
    [Required, StringLength(200)] string Apellidos,
    [Required, StringLength(10)] string NroTelefono,
    [Required, StringLength(200)] string Direccion,
    [Required, StringLength(10)] string Ubigeo);

public record DenunciadoDto(
    [StringLength(200)] string? NombreApelativo,
    string? Caracteristica);

public record IncidenteDto(
    [Required] DateTime FechaIncidente,
    [Required, StringLength(10)] string UbigeoIncidente,
    [StringLength(200)] string? DireccionIncidente,
    [Required, StringLength(30)] string Latitud,
    [Required, StringLength(30)] string Longitud,
    [StringLength(200)] string? Referencia);

public record EvidenciaDto(
    [Required, StringLength(200)] string NombreArchivo,
    [Required] string Url);

public record CreateDenunciaRequestDto(
    [Required] DateTime FechaDenuncia,
    int? IdTipoDenuncia,
    [Required] DenuncianteDto Denunciante,
    [Required] DenunciadoDto Denunciado,
    [Required] IncidenteDto Incidente,
    [Required, MinLength(1)] List<EvidenciaDto> Evidencias);

public record DenunciaDto(
    int Id,
    DateTime FechaDenuncia,
    int? IdTipoDenuncia,
    DenuncianteDto Denunciante,
    DenunciadoDto Denunciado,
    IncidenteDto Incidente,
    List<EvidenciaDto> Evidencias);

public record DenunciaResumenDto(
    int Id,
    DateTime FechaDenuncia,
    int? IdTipoDenuncia);
