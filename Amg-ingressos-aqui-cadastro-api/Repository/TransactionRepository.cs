using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using System.Diagnostics.CodeAnalysis;
using Amg_ingressos_aqui_cadastro_api.Infra;
using MongoDB.Driver;
using Amg_ingressos_aqui_cadastro_api.Repository.Querys;
using MongoDB.Bson;
using Amg_ingressos_aqui_cadastro_api.Model.Querys;

namespace Amg_ingressos_aqui_cadastro_api.Repository
{
    [ExcludeFromCodeCoverage]
    public class TransactionRepository<T> : ITransactionRepository
    {
        private readonly IMongoCollection<Transaction> _transactionCollection;
        public TransactionRepository(IDbConnection<Transaction> dbconnection)
        {
            _transactionCollection = dbconnection.GetConnection("transaction");
        }

        public async Task<object> GetById(string idTransaction)
        {
            try
            {
                var json = QuerysMongo.GetTransactionQuery;

                BsonDocument documentFilter = BsonDocument.Parse(@"{$addFields:{'_id': { '$toString': '$_id' }}}");
                BsonDocument documentFilter1 = BsonDocument.Parse(@"{ $match: { '$and': [{ '_id': '" + idTransaction.ToString() + "' }] }}");
                BsonDocument document = BsonDocument.Parse(json);
                BsonDocument[] pipeline = new BsonDocument[] {
                    documentFilter,
                    documentFilter1,
                    document
                };
                var result = _transactionCollection
                                                .Aggregate<object>(pipeline).ToList();

                List<GetTransaction> pResults = _transactionCollection
                                                .Aggregate<GetTransaction>(pipeline).ToList();

                
                //var result = await _eventCollection.FindAsync<Event>(x => x._Id == id as string)
                //    .Result.FirstOrDefaultAsync();


                if (pResults == null)
                    throw new GetByIdTransactionException("Evento não encontrado");

                return pResults;
            }
            catch (GetByIdTransactionException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}