using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Services.Interfaces
{
    public interface IEventColabService
    {
        Task<MessageReturn> CheckAllColabsOfEventAsync(string idEvent, List<string> idColabsOfProducer);
        Task<MessageReturn> SaveAsync(EventColabDTO receiptAccountSave);
        Task<bool> DoesIdExists(string idEventColab);
        Task<MessageReturn> DeleteAsync(string id);
    }
}