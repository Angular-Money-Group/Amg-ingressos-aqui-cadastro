using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Infra;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_cadastro_api.Repository
{
    public class ReceiptAccountRepository : IReceiptAccountRepository
    {
        private readonly IMongoCollection<ReceiptAccount> _receiptAccountCollection;

        public ReceiptAccountRepository(IDbConnection dbConnection)
        {
            _receiptAccountCollection = dbConnection.GetConnection<ReceiptAccount>("receiptAccount");
        }

        public async Task<ReceiptAccount> Save(ReceiptAccount receiptAccountComplet)
        {

            await _receiptAccountCollection.InsertOneAsync(receiptAccountComplet);

            if (receiptAccountComplet.Id is null)
                throw new RuleException("Erro ao salvar conta de recebimento");

            return receiptAccountComplet;
        }

        public async Task<bool> DoesValueExistsOnField<T>(string fieldName, object value)
        {

            var filter = Builders<ReceiptAccount>.Filter.Eq(fieldName, value);
            var receiptAccount = await _receiptAccountCollection.Find(filter).FirstOrDefaultAsync();
            if (receiptAccount is null)
                return false;
            return true;
        }

        public async Task<List<T>> FindByField<T>(string fieldName, object value)
        {
            FilterDefinition<ReceiptAccount> filter;
            if (fieldName == "_id")
                filter = Builders<ReceiptAccount>.Filter.Eq(fieldName, ObjectId.Parse(value.ToString()));
            else
                filter = Builders<ReceiptAccount>.Filter.Eq(fieldName, value);

            var receiptAccount = await _receiptAccountCollection
                                                    .Find(filter)
                                                    .As<T>()
                                                    .ToListAsync();

            return receiptAccount;
        }

        public async Task<bool> Delete(object id)
        {

            var result = await _receiptAccountCollection.DeleteOneAsync(x => x.Id == id as string);
            if (result.DeletedCount <= 0)
                throw new EditException("Conta de Recebimento não encontrada.");

            return true;
        }

        public async Task<List<T>> GetAllReceiptAccounts<T>()
        {
            var result = await _receiptAccountCollection
                                    .Find(_ => true)
                                    .As<T>()
                                    .ToListAsync();
            if (!result.Any())
                throw new RuleException("Contas de Recebimento não encontradas");

            return result;
        }
    }
}