using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Repository.Interfaces
{
    public interface IReceiptAccountRepository
    {
        Task<ReceiptAccount> Save(ReceiptAccount receiptAccountComplet);
        Task<bool> DoesValueExistsOnField<T>(string fieldName, object value);
        Task<List<T>> GetByField<T>(string fieldName, object value);
        Task<bool> Delete(object id);
        Task<List<T>> GetAllReceiptAccounts<T>();
    }
}