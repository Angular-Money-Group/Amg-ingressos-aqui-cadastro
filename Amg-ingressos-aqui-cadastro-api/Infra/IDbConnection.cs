using MongoDB.Driver;

namespace Amg_ingressos_aqui_cadastro_api.Infra
{
    public interface IDbConnection
    {
        IMongoCollection<T> GetConnection<T>(string colletionName);
        IMongoCollection<T> GetConnection<T>();
    }
}