namespace Amg_ingressos_aqui_cadastro_api.Repository.Interfaces
{
    public interface IUserRepository 
    {
        Task<object> Save<T>(object userComplet);
        // Task<object> FindById<T>(object id);
        Task<object> UpdateUser<T>(object id, object userComplet);
        // Task<object> removeValueFromArrayField<T>(object id, string fieldname, object IdValueToRemove);
        // Task<object> Delete<T>(object id);
        // Task<IEnumerable<object>> GetAllEvents<T>();
    }
}