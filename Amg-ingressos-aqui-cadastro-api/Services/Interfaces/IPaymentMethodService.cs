using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Services.Interfaces
{
    public interface IPaymentMethodService
    {
        Task<MessageReturn> GetAllPaymentMethodsAsync();
        Task<MessageReturn> FindByIdAsync(string idPaymentMethod);
        Task<MessageReturn> SaveAsync(PaymentMethodDTO paymentMethodSave);
        Task<bool> DoesIdExists(string idPaymentMethod);
        Task<MessageReturn> DeleteAsync(string id);
    }
}