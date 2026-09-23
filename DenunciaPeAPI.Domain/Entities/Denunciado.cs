namespace DenunciaPeAPI.Domain.Entities;

public class Denunciado
{
    public int Id { get; set; }
    public int IdDenuncia { get; set; }
    public string? NombreApelativo { get; set; }
    public byte[]? Caracteristica { get; set; }
}
