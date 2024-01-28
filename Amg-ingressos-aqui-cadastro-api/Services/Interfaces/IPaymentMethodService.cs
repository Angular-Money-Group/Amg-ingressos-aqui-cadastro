using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Services.Interfaces
{
    public interface IPaymentMethodService
    {
        Task<MessageReturn> GetAllPaymentMethodsAsync();
        Task<MessageReturn> GetByIdAsync(string idPaymentMethod);
        Task<MessageReturn> SaveAsync(PaymentMethodDto paymentMethodSave);
        Task<MessageReturn> DoesIdExists(string idPaymentMethod);
        Task<MessageReturn> DeleteAsync(string id, string idUser);
    }
}