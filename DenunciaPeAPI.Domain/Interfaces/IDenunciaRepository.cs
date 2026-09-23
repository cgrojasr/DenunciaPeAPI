using DenunciaPeAPI.Domain.Entities;

namespace DenunciaPeAPI.Domain.Interfaces;

public interface IDenunciaRepository
{
    Task<IEnumerable<Denuncia>> GetAllAsync();
    Task<DenunciaCompuesta?> GetByIdAsync(int id);

    // Inserta la denuncia y sus entidades relacionadas en una unica transaccion.
    Task<int> CreateAsync(DenunciaCompuesta denuncia);
    Task<bool> DeleteAsync(int id);
}
