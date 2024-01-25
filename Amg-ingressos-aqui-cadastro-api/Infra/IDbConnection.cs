using MongoDB.Driver;

namespace Amg_ingressos_aqui_cadastro_api.Infra
{
    public interface IDbConnection<T>
    {
        IMongoCollection<T> GetConnection(string colletionName);
    }
}