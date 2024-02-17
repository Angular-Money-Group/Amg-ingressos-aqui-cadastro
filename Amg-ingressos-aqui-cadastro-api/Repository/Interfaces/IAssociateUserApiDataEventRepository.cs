using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Repository.Interfaces
{
    public interface IAssociateUserApiDataEventRepository
    {
        Task<AssociateUserApiDataEvent> AssociateUserApiDataToEventAsync(AssociateUserApiDataEvent data);
        Task<List<T>> GetUserApiDataToEventAsync<T>(string idEvent);
    }
}