using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Services.Interfaces
{
    public interface IEventColabService
    {
        Task<MessageReturn> GetAllEventColabsAsync();
        Task<MessageReturn> GetAllColabsOfEventAsync(string IdEvent);
        Task<MessageReturn> FindByIdAsync(string idEventColab);
        Task<MessageReturn> SaveAsync(EventColabDTO receiptAccountSave);
        Task<MessageReturn> RegisterColabAsync(string idEvent, UserDTO colab);
        Task<bool> DoesIdExists(string idEventColab);
        Task<MessageReturn> DeleteAsync(string id);
    }
}