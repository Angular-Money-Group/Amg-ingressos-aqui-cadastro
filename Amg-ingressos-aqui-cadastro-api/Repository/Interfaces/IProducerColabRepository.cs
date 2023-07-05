using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Repository.Interfaces
{
    public interface IProducerColabRepository 
    {
        Task<object> Save<T>(ProducerColab producerColabComplet);
        Task<bool> DoesValueExistsOnField<T>(string fieldName, object value);
        Task<ProducerColab> FindByField<T>(string fieldName, object value);
        Task<List<ProducerColab>> FindAllColabsOfProducer<T>(string idProducer);
        Task<object> Delete<T>(object id);
    }
}