using DenunciaPeAPI.Application.DTOs;

namespace DenunciaPeAPI.Application.Interfaces;

public interface IDenunciaService
{
    Task<IEnumerable<DenunciaResumenDto>> GetAllAsync();
    Task<DenunciaDto?> GetByIdAsync(int id);
    Task<IEnumerable<DenunciaResumenDto>> GetByNroDocumentoAsync(string nroDocumento);

    // Valida la estructura de la solicitud y, si es valida, persiste la denuncia y sus relaciones.
    Task<DenunciaDto> CreateAsync(CreateDenunciaRequestDto dto);
    Task<bool> DeleteAsync(int id);
}
