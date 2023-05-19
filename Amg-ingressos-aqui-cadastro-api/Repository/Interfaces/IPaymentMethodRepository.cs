using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Repository.Interfaces
{
    public interface IPaymentMethodRepository 
    {
        Task<object> Save<T>(PaymentMethod paymentMethodComplet);
        Task<bool> DoesValueExistsOnField<T>(string fieldName, object value);
        Task<PaymentMethod> FindByField<T>(string fieldName, object value);
        Task<object> Delete<T>(object id);
        Task<List<PaymentMethod>> GetAllPaymentMethods<T>();
    }
}