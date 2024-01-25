using Amg_ingressos_aqui_cadastro_api.Infra;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_cadastro_api.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly IMongoCollection<Email> _emailCollection;
        public EmailRepository(IDbConnection<Email> dbconnectionIten)
        {
            _emailCollection = dbconnectionIten.GetConnection("templateemails");
        }
        public async Task<object> SaveAsync(object email)
        {
            await _emailCollection.InsertOneAsync((Email)email);
            return (Email)email;
        }

        public async Task<object> SaveManyAsync(List<Email> emails)
        {
            await _emailCollection.InsertManyAsync(emails);
            return emails;
        }
    }
}