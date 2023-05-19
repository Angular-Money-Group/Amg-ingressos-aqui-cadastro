using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Repository.Interfaces
{
    public interface IEventColabRepository 
    {
        Task<object> Save<T>(EventColab eventColabComplet);
        Task<bool> DoesValueExistsOnField<T>(string fieldName, object value);
        Task<EventColab> FindByField<T>(string fieldName, object value);
        Task<object> Delete<T>(object id);
        Task<List<EventColab>> GetAllEventColabs<T>();
    }
}