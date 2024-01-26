using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Repository.Interfaces
{
    public interface IEventColabRepository
    {
        Task<EventColab> Save(EventColab eventColabComplet);
        Task<bool> DoesValueExistsOnField<T>(string fieldName, object value);
        Task<T> FindByField<T>(string fieldName, object value);
        Task<bool> Delete(object id);
        Task<List<string>> FindAllColabsOfEvent(string idEvent);
    }
}