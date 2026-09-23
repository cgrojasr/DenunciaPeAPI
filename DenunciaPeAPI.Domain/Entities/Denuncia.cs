namespace DenunciaPeAPI.Domain.Entities;

public class Denuncia
{
    public int Id { get; set; }
    public DateTime FechaDenuncia { get; set; }
    public int? IdTipoDenuncia { get; set; }
}
