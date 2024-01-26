using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Infra;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_cadastro_api.Repository
{
    public class PaymentMethodRepository : IPaymentMethodRepository
    {
        private readonly IMongoCollection<PaymentMethod> _paymentMethodCollection;

        public PaymentMethodRepository(IDbConnection dbConnection)
        {
            _paymentMethodCollection = dbConnection.GetConnection<PaymentMethod>("paymentMethod");
        }

        public async Task<PaymentMethod> Save<T>(PaymentMethod paymentMethodComplet)
        {
            await _paymentMethodCollection.InsertOneAsync(paymentMethodComplet);

            if (paymentMethodComplet.Id is null)
                throw new RuleException("Erro ao salvar método de pagamento");

            return paymentMethodComplet;
        }

        public async Task<bool> DoesValueExistsOnField<T>(string fieldName, object value)
        {
            var filter = Builders<PaymentMethod>.Filter.Eq(fieldName, value);
            var paymentMethod = await _paymentMethodCollection.Find(filter).FirstOrDefaultAsync();
            if (paymentMethod is null)
                return false;
            return true;
        }

        public async Task<T> FindByField<T>(string fieldName, object value)
        {
            var filter = Builders<PaymentMethod>.Filter.Eq(fieldName, value);
            var paymentMethod = await _paymentMethodCollection
                                            .Find(filter)
                                            .As<T>()
                                            .FirstOrDefaultAsync();
            if (paymentMethod == null)
                throw new RuleException("Método de Pagamento não encontrado por " + fieldName + ".");

            return paymentMethod;
        }

        public async Task<bool> Delete<T>(object id)
        {
            var result = await _paymentMethodCollection.DeleteOneAsync(x => x.Id == id as string);
            if (result.DeletedCount <= 0)
                throw new DeleteException("Método de Pagamento não encontrado.");

            return true;
        }

        public async Task<List<T>> GetAllPaymentMethods<T>()
        {
            var result = await _paymentMethodCollection
                                                    .Find(_ => true)
                                                    .As<T>()
                                                    .ToListAsync();
            if (!result.Any())
                throw new RuleException("Métodos de Pagamento não encontrados");

            return result;
        }
    }
}