using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Services.Interfaces
{
    public interface ISupportService
    {
        Task<MessageReturn> GetAllAsync();
        Task<MessageReturn> FindByIdAsync(string id);
        Task<MessageReturn> SaveAsync(SupportDTO supportSave);
        Task<MessageReturn> UpdateByIdAsync(string id, SupportDTO ticketSupport);
        Task<MessageReturn> DeleteAsync(string id);
    }
}