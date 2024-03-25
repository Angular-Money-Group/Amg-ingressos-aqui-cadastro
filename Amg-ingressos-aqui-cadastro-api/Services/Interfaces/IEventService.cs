using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Services.Interfaces
{
    public interface IEventService
    {
        Task<MessageReturn> GetById(string idEvent);
    }
}