namespace DenunciaPeAPI.Domain.Entities;

// Registra intentos de registro de denuncias, exitosos o rechazados por validacion.
public class AuditoriaEvento
{
    public int Id { get; set; }
    public DateTime FechaEvento { get; set; } = DateTime.UtcNow;
    public string Entidad { get; set; } = string.Empty;
    public string Evento { get; set; } = string.Empty;
    public bool Exitoso { get; set; }
    public string? Detalle { get; set; }
}
