namespace DenunciaPeAPI.Domain.Entities;

// Agrega una denuncia junto con sus entidades relacionadas obligatorias.
public class DenunciaCompuesta
{
    public required Denuncia Denuncia { get; init; }
    public required Denunciante Denunciante { get; init; }
    public required Denunciado Denunciado { get; init; }
    public required Incidente Incidente { get; init; }
    public required IReadOnlyList<Evidencia> Evidencias { get; init; }
}
