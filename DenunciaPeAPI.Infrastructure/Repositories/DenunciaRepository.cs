using Dapper;
using DenunciaPeAPI.Domain.Entities;
using DenunciaPeAPI.Domain.Interfaces;
using Microsoft.Data.SqlClient;

namespace DenunciaPeAPI.Infrastructure.Repositories;

public class DenunciaRepository(IDbConnectionFactory connectionFactory) : IDenunciaRepository
{
    public async Task<IEnumerable<Denuncia>> GetAllAsync()
    {
        using var connection = connectionFactory.CreateConnection();
        const string sql = """
            SELECT id_denuncia AS Id, fecha_denuncia AS FechaDenuncia, id_tipo_denuncia AS IdTipoDenuncia
            FROM denuncia
            """;
        return await connection.QueryAsync<Denuncia>(sql);
    }

    public async Task<DenunciaCompuesta?> GetByIdAsync(int id)
    {
        using var connection = connectionFactory.CreateConnection();

        const string denunciaSql = """
            SELECT id_denuncia AS Id, fecha_denuncia AS FechaDenuncia, id_tipo_denuncia AS IdTipoDenuncia
            FROM denuncia WHERE id_denuncia = @Id
            """;
        var denuncia = await connection.QueryFirstOrDefaultAsync<Denuncia>(denunciaSql, new { Id = id });
        if (denuncia is null)
        {
            return null;
        }

        const string denuncianteSql = """
            SELECT id_denunciante AS Id, id_denuncia AS IdDenuncia, tipo_documento AS TipoDocumento,
                   nro_documento AS NroDocumento, nombres AS Nombres, apellidos AS Apellidos,
                   nro_telefono AS NroTelefono, direccion AS Direccion, ubigeo AS Ubigeo
            FROM denunciante WHERE id_denuncia = @Id
            """;
        const string denunciadoSql = """
            SELECT id_denunciado AS Id, id_denuncia AS IdDenuncia, nombre_apelativo AS NombreApelativo,
                   caracteristica AS Caracteristica
            FROM denunciado WHERE id_denuncia = @Id
            """;
        const string incidenteSql = """
            SELECT id_incidente AS Id, id_denuncia AS IdDenuncia, fecha_incidente AS FechaIncidente,
                   ubigeo_incidente AS UbigeoIncidente, direccion_incidente AS DireccionIncidente,
                   latitud AS Latitud, longitud AS Longitud, referencia AS Referencia
            FROM incidente WHERE id_denuncia = @Id
            """;
        const string evidenciaSql = """
            SELECT id_evidencia AS Id, id_denuncia AS IdDenuncia, nombre_archivo AS NombreArchivo, url AS Url
            FROM evidencia WHERE id_denuncia = @Id
            """;

        var denunciante = await connection.QueryFirstOrDefaultAsync<Denunciante>(denuncianteSql, new { Id = id });
        var denunciado = await connection.QueryFirstOrDefaultAsync<Denunciado>(denunciadoSql, new { Id = id });
        var incidente = await connection.QueryFirstOrDefaultAsync<Incidente>(incidenteSql, new { Id = id });
        var evidencias = await connection.QueryAsync<Evidencia>(evidenciaSql, new { Id = id });

        if (denunciante is null || denunciado is null || incidente is null)
        {
            return null;
        }

        return new DenunciaCompuesta
        {
            Denuncia = denuncia,
            Denunciante = denunciante,
            Denunciado = denunciado,
            Incidente = incidente,
            Evidencias = evidencias.ToList()
        };
    }

    public async Task<int> CreateAsync(DenunciaCompuesta denuncia)
    {
        using var connection = (SqlConnection)connectionFactory.CreateConnection();
        await connection.OpenAsync();
        using var transaction = connection.BeginTransaction();
        try
        {
            const string insertDenunciaSql = """
                INSERT INTO denuncia (fecha_denuncia, id_tipo_denuncia)
                OUTPUT INSERTED.id_denuncia
                VALUES (@FechaDenuncia, @IdTipoDenuncia)
                """;
            var idDenuncia = await connection.ExecuteScalarAsync<int>(
                insertDenunciaSql, denuncia.Denuncia, transaction);

            const string insertDenuncianteSql = """
                INSERT INTO denunciante
                    (id_denuncia, tipo_documento, nro_documento, nombres, apellidos, nro_telefono, direccion, ubigeo)
                VALUES
                    (@IdDenuncia, @TipoDocumento, @NroDocumento, @Nombres, @Apellidos, @NroTelefono, @Direccion, @Ubigeo)
                """;
            await connection.ExecuteAsync(insertDenuncianteSql, new
            {
                IdDenuncia = idDenuncia,
                denuncia.Denunciante.TipoDocumento,
                denuncia.Denunciante.NroDocumento,
                denuncia.Denunciante.Nombres,
                denuncia.Denunciante.Apellidos,
                denuncia.Denunciante.NroTelefono,
                denuncia.Denunciante.Direccion,
                denuncia.Denunciante.Ubigeo
            }, transaction);

            const string insertDenunciadoSql = """
                INSERT INTO denunciado (id_denuncia, nombre_apelativo, caracteristica)
                VALUES (@IdDenuncia, @NombreApelativo, @Caracteristica)
                """;
            await connection.ExecuteAsync(insertDenunciadoSql, new
            {
                IdDenuncia = idDenuncia,
                denuncia.Denunciado.NombreApelativo,
                denuncia.Denunciado.Caracteristica
            }, transaction);

            const string insertIncidenteSql = """
                INSERT INTO incidente
                    (id_denuncia, fecha_incidente, ubigeo_incidente, direccion_incidente, latitud, longitud, referencia)
                VALUES
                    (@IdDenuncia, @FechaIncidente, @UbigeoIncidente, @DireccionIncidente, @Latitud, @Longitud, @Referencia)
                """;
            await connection.ExecuteAsync(insertIncidenteSql, new
            {
                IdDenuncia = idDenuncia,
                denuncia.Incidente.FechaIncidente,
                denuncia.Incidente.UbigeoIncidente,
                denuncia.Incidente.DireccionIncidente,
                denuncia.Incidente.Latitud,
                denuncia.Incidente.Longitud,
                denuncia.Incidente.Referencia
            }, transaction);

            const string insertEvidenciaSql = """
                INSERT INTO evidencia (id_denuncia, nombre_archivo, url)
                VALUES (@IdDenuncia, @NombreArchivo, @Url)
                """;
            foreach (var evidencia in denuncia.Evidencias)
            {
                await connection.ExecuteAsync(insertEvidenciaSql, new
                {
                    IdDenuncia = idDenuncia,
                    evidencia.NombreArchivo,
                    evidencia.Url
                }, transaction);
            }

            transaction.Commit();
            return idDenuncia;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = (SqlConnection)connectionFactory.CreateConnection();
        await connection.OpenAsync();
        using var transaction = connection.BeginTransaction();
        try
        {
            await connection.ExecuteAsync("DELETE FROM evidencia WHERE id_denuncia = @Id", new { Id = id }, transaction);
            await connection.ExecuteAsync("DELETE FROM incidente WHERE id_denuncia = @Id", new { Id = id }, transaction);
            await connection.ExecuteAsync("DELETE FROM denunciado WHERE id_denuncia = @Id", new { Id = id }, transaction);
            await connection.ExecuteAsync("DELETE FROM denunciante WHERE id_denuncia = @Id", new { Id = id }, transaction);
            var rows = await connection.ExecuteAsync("DELETE FROM denuncia WHERE id_denuncia = @Id", new { Id = id }, transaction);

            transaction.Commit();
            return rows > 0;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}
