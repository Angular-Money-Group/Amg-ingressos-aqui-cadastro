using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Repository.Interfaces
{
    public interface IPaymentMethodRepository
    {
        Task<PaymentMethod> Save<T>(PaymentMethod paymentMethodComplet);
        Task<bool> DoesValueExistsOnField<T>(string fieldName, object value);
        Task<T> FindByField<T>(string fieldName, object value);
        Task<bool> Delete<T>(object id);
        Task<List<T>> GetAllPaymentMethods<T>();
    }
}