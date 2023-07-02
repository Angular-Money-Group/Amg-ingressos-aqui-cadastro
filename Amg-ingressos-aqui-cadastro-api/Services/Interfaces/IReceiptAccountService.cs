using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Services.Interfaces
{
    public interface IReceiptAccountService
    {
        Task<MessageReturn> GetAllReceiptAccountsAsync();
        Task<MessageReturn> FindByIdAsync(string idReceiptAccount);
        Task<MessageReturn> SaveAsync(ReceiptAccountDTO receiptAccountSave);
        Task<bool> DoesIdExists(string idReceiptAccount);
        Task<MessageReturn> DeleteAsync(string id, string idUser);
    }
}