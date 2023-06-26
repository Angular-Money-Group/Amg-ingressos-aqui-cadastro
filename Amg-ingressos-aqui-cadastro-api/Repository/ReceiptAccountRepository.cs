using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using System.Diagnostics.CodeAnalysis;
using Amg_ingressos_aqui_cadastro_api.Infra;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_cadastro_api.Repository
{
    public class ReceiptAccountRepository<T> : IReceiptAccountRepository
    {
        private readonly IMongoCollection<ReceiptAccount> _receiptAccountCollection;

        public ReceiptAccountRepository(IDbConnection<ReceiptAccount> dbConnection) {
            this._receiptAccountCollection = dbConnection.GetConnection("receiptAccount");
        }
        
        public async Task<object> Save<T>(ReceiptAccount receiptAccountComplet) {
            try {
                await this._receiptAccountCollection.InsertOneAsync(receiptAccountComplet);

                if (receiptAccountComplet.Id is null)
                    throw new SaveReceiptAccountException("Erro ao salvar conta de recebimento");

                return receiptAccountComplet.Id;
            }
            catch (SaveReceiptAccountException ex) {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DoesValueExistsOnField<T>(string fieldName, object value) {
            try {
                var filter = Builders<ReceiptAccount>.Filter.Eq(fieldName, value);
                var receiptAccount = await _receiptAccountCollection.Find(filter).FirstOrDefaultAsync();
                if (receiptAccount is null)
                    return false;
                return true;
            }   
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ReceiptAccount> FindByField<T>(string fieldName, object value) {
            try {

                var filter = Builders<ReceiptAccount>.Filter.Eq(fieldName, value);
                var receiptAccount = await _receiptAccountCollection.Find(filter).FirstOrDefaultAsync();
                if (receiptAccount is not null)
                    return receiptAccount;
                else
                    throw new ReceiptAccountNotFound("Conta de Recebimento não encontrada por " + fieldName + ".");
            }
            catch (ReceiptAccountNotFound ex) {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public async Task<object> Delete<T>(object id) {
            try
            {
                var result = await _receiptAccountCollection.DeleteOneAsync(x => x.Id == id as string);
                if (result.DeletedCount >= 1)
                    return "Conta de Recebimento Deletada.";
                else
                    throw new DeleteReceiptAccountException("Conta de Recebimento não encontrada.");
            }
            catch (DeleteReceiptAccountException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ReceiptAccount>> GetAllReceiptAccounts<T>() {
            try
            {
                List<ReceiptAccount> result = await _receiptAccountCollection.Find(_ => true).ToListAsync();
                if (!result.Any())
                    throw new GetAllReceiptAccountException("Contas de Recebimento não encontradas");

                return result;
            }
            catch (GetAllReceiptAccountException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}