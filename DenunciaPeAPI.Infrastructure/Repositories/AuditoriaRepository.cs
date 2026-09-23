using Dapper;
using DenunciaPeAPI.Domain.Entities;
using DenunciaPeAPI.Domain.Interfaces;

namespace DenunciaPeAPI.Infrastructure.Repositories;

public class AuditoriaRepository(IDbConnectionFactory connectionFactory) : IAuditoriaRepository
{
    public async Task CreateAsync(AuditoriaEvento evento)
    {
        using var connection = connectionFactory.CreateConnection();
        const string sql = """
            INSERT INTO auditoria (fecha_evento, entidad, evento, exitoso, detalle)
            VALUES (@FechaEvento, @Entidad, @Evento, @Exitoso, @Detalle)
            """;
        await connection.ExecuteAsync(sql, evento);
    }
}
