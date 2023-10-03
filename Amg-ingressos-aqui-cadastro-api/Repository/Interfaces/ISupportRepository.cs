using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Repository.Interfaces
{
    public interface ISupportRepository

    {
        Task<List<TicketSupport>> GetAll<T>();
        Task<TicketSupport> FindById<T>(string id);
        Task<TicketSupport> Save<T>(TicketSupport ticketSupport);
        Task<TicketSupport> UpdateByIdAsync<T>(string id, SupportDTO ticketSupport);
        Task<string> DeleteAsync<T>(string id);
    }
}
