namespace Amg_ingressos_aqui_cadastro_api.Repository.Interfaces
{
    public interface IEventRepository
    {
        Task<List<T>> FindById<T>(string id);
    }
}