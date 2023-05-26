namespace Amg_ingressos_aqui_cadastro_api.Repository.Interfaces
{
    public interface ITransactionRepository 
    {
        Task<object> GetById(string idTransaction);
    }
}