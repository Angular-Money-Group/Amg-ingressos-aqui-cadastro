using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Repository.Interfaces
{
    public interface IReceiptAccountRepository 
    {
        Task<object> Save<T>(ReceiptAccount receiptAccountComplet);
        Task<bool> DoesValueExistsOnField<T>(string fieldName, object value);
        Task<ReceiptAccount> FindByField<T>(string fieldName, object value);
        Task<object> Delete<T>(object id);
        Task<List<ReceiptAccount>> GetAllReceiptAccounts<T>();
    }
}