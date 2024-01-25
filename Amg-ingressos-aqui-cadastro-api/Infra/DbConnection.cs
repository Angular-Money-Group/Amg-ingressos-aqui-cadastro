using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_cadastro_api.Infra
{
    public class DbConnection<T> : IDbConnection<T>
    {
        private readonly IOptions<CadastroDatabaseSettings> _config;
        public DbConnection(IOptions<CadastroDatabaseSettings> transactionDatabaseSettings)
        {
            _config = transactionDatabaseSettings;
        }

        public IMongoCollection<T> GetConnection(string colletionName){

            var mongoUrl = new MongoUrl(_config.Value.ConnectionString);
            var _mongoClient = new MongoClient(mongoUrl);
            var mongoDatabase = _mongoClient.GetDatabase(_config.Value.DatabaseName);

            return mongoDatabase.GetCollection<T>(colletionName);
        }
    }
}