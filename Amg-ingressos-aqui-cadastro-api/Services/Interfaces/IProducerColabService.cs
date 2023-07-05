using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Services.Interfaces
{
    public interface IProducerColabService
    {
        Task<MessageReturn> GetAllColabsOfProducerAsync(string IdProducer);
        Task<MessageReturn> GetIdColabsOfProducerAsync(string idProducer);
        Task<MessageReturn> SaveAsync(ProducerColabDTO producerColabSaveDTO);
        Task<MessageReturn> RegisterColabAsync(string idProducer, UserDTO colab);
        Task<bool> DoesIdExists(string idProducerColab);
        Task<MessageReturn> DeleteAsync(string id);
    }
}