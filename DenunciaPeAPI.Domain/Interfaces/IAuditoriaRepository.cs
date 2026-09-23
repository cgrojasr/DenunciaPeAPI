using DenunciaPeAPI.Domain.Entities;

namespace DenunciaPeAPI.Domain.Interfaces;

public interface IAuditoriaRepository
{
    Task CreateAsync(AuditoriaEvento evento);
}
