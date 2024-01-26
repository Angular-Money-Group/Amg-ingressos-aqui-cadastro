using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Repository.Interfaces
{
    public interface ISupportRepository

    {
        Task<List<T>> GetAll<T>();
        Task<T> FindById<T>(string id);
        Task<TicketSupport> Save(TicketSupport ticketSupport);
        Task<TicketSupport> UpdateByIdAsync(string id, TicketSupport ticketSupport);
        Task<bool> DeleteAsync(string id);
    }
}