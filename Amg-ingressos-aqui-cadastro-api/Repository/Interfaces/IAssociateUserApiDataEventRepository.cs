using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Repository.Interfaces
{
    public interface IAssociateUserApiDataEventRepository
    {
        Task<object?> AssociateUserApiDataToEventAsync(AssociateUserApiDataEvent data);
        Task<object?> GetUserApiDataToEventAsync(string idEvent);
    }
}