using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Repository.Interfaces
{
    public interface IUserRepository 
    {
        Task<object> Save<T>(User userComplet);
        Task<bool> DoesValueExistsOnField<T>(string fieldName, object value);
        Task<User> GetUser(string id);
        Task<User> FindByField<T>(string fieldName, object value);
        Task<object> UpdateUser<T>(object id, User userComplet);
        Task<object> UpdatePasswordUser<T>(string id, string password);
        Task<object> Delete<T>(object id);
        Task<List<User>> Get<T>(string email, string type);
    }
}