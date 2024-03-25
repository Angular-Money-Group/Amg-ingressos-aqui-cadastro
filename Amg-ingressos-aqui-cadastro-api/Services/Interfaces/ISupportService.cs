using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Services.Interfaces
{
    public interface ISupportService
    {
        Task<MessageReturn> GetAllAsync();
        Task<MessageReturn> FindByIdAsync(string id);
        Task<MessageReturn> SaveAsync(TicketSupportDto supportSave);
        Task<MessageReturn> EditByIdAsync(string id, TicketSupportDto ticketSupport);
        Task<MessageReturn> DeleteAsync(string id);
    }
}