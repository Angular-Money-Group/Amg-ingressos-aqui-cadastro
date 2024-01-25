namespace Amg_ingressos_aqui_cadastro_api.Repository.Interfaces
{
    public interface ISequenceRepository
    {
        Task<long> GetNextSequenceValue(string sequenceName);
    }
}
