namespace DenunciaPeAPI.Domain.Entities;

public class Evidencia
{
    public int Id { get; set; }
    public int IdDenuncia { get; set; }
    public string NombreArchivo { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}
