using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Infra;
using MongoDB.Driver;
using Amg_ingressos_aqui_cadastro_api.Enum;
using System;
using MongoDB.Bson;

namespace Amg_ingressos_aqui_cadastro_api.Repository
{
    public class SequenceRepository : ISequenceRepository
    {
        private readonly IMongoCollection<Sequence> _sequenceCollection;

        public SequenceRepository(IDbConnection<Sequence> dbConnection)
        {
            this._sequenceCollection = dbConnection.GetConnection("sequence");
        }

        public async Task<long> GetNextSequenceValue(string sequenceName)
        {
            var filter = Builders<Sequence>.Filter.Eq(s => s.Id, sequenceName);
            var update = Builders<Sequence>.Update.Inc(s => s.Value, 1);
            var options = new FindOneAndUpdateOptions<Sequence>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After
            };

            var sequence = _sequenceCollection.FindOneAndUpdate(filter, update, options);

            return sequence.Value;
        }
    }
}