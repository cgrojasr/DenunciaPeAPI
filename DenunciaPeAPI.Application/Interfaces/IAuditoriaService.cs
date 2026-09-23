namespace DenunciaPeAPI.Application.Interfaces;

public interface IAuditoriaService
{
    Task RegistrarExitoAsync(string entidad, string detalle);
    Task RegistrarRechazoAsync(string entidad, IReadOnlyCollection<string> errores);
}
