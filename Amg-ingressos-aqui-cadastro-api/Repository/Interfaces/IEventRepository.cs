using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Repository.Interfaces
{
    public interface IEventRepository 
    {
        Task<List<Event>> FindById<T>(string id);

    }
}