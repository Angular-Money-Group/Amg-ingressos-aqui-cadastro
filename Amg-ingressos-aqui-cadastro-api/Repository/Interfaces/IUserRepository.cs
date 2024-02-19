using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<User> Save(User user);
        Task<bool> DoesValueExistsOnField(string fieldName, object value);
        Task<T> GetUser<T>(string id);
        Task<T> GetByField<T>(string fieldName, object value);
        Task<User> UpdateUser(string id, User user);
        Task<bool> UpdatePasswordUser(string id, string password);
        Task<bool> Delete(object id);
        Task<List<T>> Get<T>(FiltersUser filters);
        Task<object> UpdateUserConfirmation<T>(string id, UserConfirmation userConfirmation);
    }
}