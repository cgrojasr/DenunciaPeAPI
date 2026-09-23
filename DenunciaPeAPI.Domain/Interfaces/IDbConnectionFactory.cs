using System.Data;

namespace DenunciaPeAPI.Domain.Interfaces;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
