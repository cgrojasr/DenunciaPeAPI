namespace DenunciaPeAPI.Domain.Entities;

public class Incidente
{
    public int Id { get; set; }
    public int IdDenuncia { get; set; }
    public DateTime FechaIncidente { get; set; }
    public string UbigeoIncidente { get; set; } = string.Empty;
    public string? DireccionIncidente { get; set; }
    public string Latitud { get; set; } = string.Empty;
    public string Longitud { get; set; } = string.Empty;
    public string? Referencia { get; set; }
}
