using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Infra;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_cadastro_api.Repository
{
    public class PaymentMethodRepository<T> : IPaymentMethodRepository
    {
        private readonly IMongoCollection<PaymentMethod> _paymentMethodCollection;

        public PaymentMethodRepository(IDbConnection<PaymentMethod> dbConnection)
        {
            _paymentMethodCollection = dbConnection.GetConnection("paymentMethod");
        }

        public async Task<object> Save<T>(PaymentMethod paymentMethodComplet)
        {
            await _paymentMethodCollection.InsertOneAsync(paymentMethodComplet);

            if (paymentMethodComplet.Id is null)
                throw new RuleException("Erro ao salvar método de pagamento");

            return paymentMethodComplet.Id;
        }

        public async Task<bool> DoesValueExistsOnField<T>(string fieldName, object value)
        {
            var filter = Builders<PaymentMethod>.Filter.Eq(fieldName, value);
            var paymentMethod = await _paymentMethodCollection.Find(filter).FirstOrDefaultAsync();
            if (paymentMethod is null)
                return false;
            return true;
        }

        public async Task<PaymentMethod> FindByField<T>(string fieldName, object value)
        {
            var filter = Builders<PaymentMethod>.Filter.Eq(fieldName, value);
            var paymentMethod = await _paymentMethodCollection.Find(filter).FirstOrDefaultAsync();
            if (paymentMethod is not null)
                return paymentMethod;
            else
                throw new RuleException("Método de Pagamento não encontrado por " + fieldName + ".");
        }

        public async Task<object> Delete<T>(object id)
        {

            var result = await _paymentMethodCollection.DeleteOneAsync(x => x.Id == id as string);
            if (result.DeletedCount >= 1)
                return "Método de Pagamento Deletado.";
            else
                throw new DeleteException("Método de Pagamento não encontrado.");
        }

        public async Task<List<PaymentMethod>> GetAllPaymentMethods<T>()
        {
            List<PaymentMethod> result = await _paymentMethodCollection.Find(_ => true).ToListAsync();
            if (!result.Any())
                throw new RuleException("Métodos de Pagamento não encontrados");

            return result;
        }
    }
}