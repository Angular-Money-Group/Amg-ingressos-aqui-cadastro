namespace Amg_ingressos_aqui_cadastro_api.Repository.Interfaces
{
    public interface IUserRepository 
    {
        Task<object> Save<T>(object userComplet);
        Task<bool> DoesValueExistsOnField<T>(string fieldName, T value);
        Task<object> FindByField<T>(string value, string fieldname);
        Task<object> UpdateUser<T>(object id, object userComplet);
        Task<object> removeValueFromArrayField<T>(object id, object fieldname, object IdValueToRemove);
        Task<object> Delete<T>(object id);
        Task<IEnumerable<object>> GetAllUsers<T>();
    }
}