using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_cadastro_api.Infra
{
    public class DbConnection : IDbConnection
    {
        private readonly IOptions<CadastroDatabaseSettings> _config;
        public DbConnection(IOptions<CadastroDatabaseSettings> cadastroDatabaseSettings)
        {
            _config = cadastroDatabaseSettings;
        }

        public IMongoCollection<T> GetConnection<T>(string colletionName)
        {
            var mongoUrl = new MongoUrl(_config.Value.ConnectionString);
            var mongoClient = new MongoClient(mongoUrl);
            var mongoDatabase = mongoClient.GetDatabase(_config.Value.DatabaseName);

            return mongoDatabase.GetCollection<T>(colletionName);
        }

        public IMongoCollection<T> GetConnection<T>()
        {
            var colletionName = GetCollectionName<T>();
            var mongoUrl = new MongoUrl(_config.Value.ConnectionString);
            var mongoClient = new MongoClient(mongoUrl);
            var mongoDatabase = mongoClient.GetDatabase(_config.Value.DatabaseName);

            return mongoDatabase.GetCollection<T>(colletionName);
        }

        private static string GetCollectionName<T>()
        {
            return typeof(T).Name.ToLower() ?? string.Empty;
        }
    }
}