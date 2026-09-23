namespace DenunciaPeAPI.Domain.Entities;

public class Denunciante
{
    public int Id { get; set; }
    public int IdDenuncia { get; set; }
    public string TipoDocumento { get; set; } = string.Empty;
    public string NroDocumento { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string NroTelefono { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Ubigeo { get; set; } = string.Empty;
}
