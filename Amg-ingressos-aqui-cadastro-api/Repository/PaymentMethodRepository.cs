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
    public class PaymentMethodRepository<T> : IPaymentMethodRepository
    {
        private readonly IMongoCollection<PaymentMethod> _paymentMethodCollection;

        public PaymentMethodRepository(IDbConnection<PaymentMethod> dbConnection) {
            this._paymentMethodCollection = dbConnection.GetConnection("paymentMethod");
        }
        
        public async Task<object> Save<T>(PaymentMethod paymentMethodComplet) {
            try {
                await this._paymentMethodCollection.InsertOneAsync(paymentMethodComplet);

                if (paymentMethodComplet.Id is null)
                    throw new SavePaymentMethodException("Erro ao salvar método de pagamento");

                return paymentMethodComplet.Id;
            }
            catch (SavePaymentMethodException ex) {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DoesValueExistsOnField<T>(string fieldName, object value) {
            try {
                var filter = Builders<PaymentMethod>.Filter.Eq(fieldName, value);
                var paymentMethod = await _paymentMethodCollection.Find(filter).FirstOrDefaultAsync();
                if (paymentMethod is null)
                    return false;
                return true;
            }   
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PaymentMethod> FindByField<T>(string fieldName, object value) {
            try {

                var filter = Builders<PaymentMethod>.Filter.Eq(fieldName, value);
                var paymentMethod = await _paymentMethodCollection.Find(filter).FirstOrDefaultAsync();
                if (paymentMethod is not null)
                    return paymentMethod;
                else
                    throw new PaymentMethodNotFound("Método de Pagamento não encontrado por " + fieldName + ".");
            }
            catch (PaymentMethodNotFound ex) {
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
                var result = await _paymentMethodCollection.DeleteOneAsync(x => x.Id == id as string);
                if (result.DeletedCount >= 1)
                    return "Método de Pagamento Deletado.";
                else
                    throw new DeletePaymentMethodException("Método de Pagamento não encontrado.");
            }
            catch (DeletePaymentMethodException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<PaymentMethod>> GetAllPaymentMethods<T>() {
            try
            {
                List<PaymentMethod> result = await _paymentMethodCollection.Find(_ => true).ToListAsync();
                if (!result.Any())
                    throw new GetAllPaymentMethodException("Métodos de Pagamento não encontrados");

                return result;
            }
            catch (GetAllPaymentMethodException ex)
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