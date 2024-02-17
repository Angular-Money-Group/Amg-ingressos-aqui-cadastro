using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Infra;
using MongoDB.Driver;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Repository
{
    public class SequenceRepository : ISequenceRepository
    {
        private readonly IMongoCollection<Sequence> _sequenceCollection;

        public SequenceRepository(IDbConnection dbConnection)
        {
            _sequenceCollection = dbConnection.GetConnection<Sequence>("sequence");
        }

        public Task<long> GetNextSequenceValue(string sequenceName)
        {
            var filter = Builders<Sequence>.Filter.Eq(s => s.Id, sequenceName);
            var update = Builders<Sequence>.Update.Inc(s => s.Value, 1);
            var options = new FindOneAndUpdateOptions<Sequence>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After
            };

            var sequence = _sequenceCollection.FindOneAndUpdate(filter, update, options);

            return Task.FromResult(sequence.Value);
        }
    }
}