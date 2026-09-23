namespace DenunciaPeAPI.Application.Exceptions;

// Se lanza cuando una solicitud de denuncia no cumple la estructura o formato requerido.
public class DenunciaValidationException(IReadOnlyCollection<string> errores)
    : Exception("La solicitud de denuncia no cumple la estructura o formato requerido.")
{
    public IReadOnlyCollection<string> Errores { get; } = errores;
}
