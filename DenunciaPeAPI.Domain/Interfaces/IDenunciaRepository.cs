using DenunciaPeAPI.Domain.Entities;

namespace DenunciaPeAPI.Domain.Interfaces;

public interface IDenunciaRepository
{
    Task<IEnumerable<Denuncia>> GetAllAsync();
    Task<DenunciaCompuesta?> GetByIdAsync(int id);
    Task<IEnumerable<DenunciaConReferencia>> GetByNroDocumentoAsync(string nroDocumento);

    // Inserta la denuncia y sus entidades relacionadas en una unica transaccion.
    Task<int> CreateAsync(DenunciaCompuesta denuncia);
    Task<bool> DeleteAsync(int id);
}
