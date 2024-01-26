using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Repository.Interfaces
{
    public interface IEmailRepository
    {
        public Task<Email> SaveAsync(Email email);
        public Task<List<Email>> SaveManyAsync(List<Email> listEmail);
    }
}