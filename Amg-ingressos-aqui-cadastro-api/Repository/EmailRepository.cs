using Amg_ingressos_aqui_cadastro_api.Infra;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_cadastro_api.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly IMongoCollection<Email> _emailCollection;
        public EmailRepository(IDbConnection dbconnectionIten)
        {
            _emailCollection = dbconnectionIten.GetConnection<Email>("templateemails");
        }
        public async Task<Email> SaveAsync(Email email)
        {
            await _emailCollection.InsertOneAsync(email);
            return email;
        }

        public async Task<List<Email>> SaveManyAsync(List<Email> listEmail)
        {
            await _emailCollection.InsertManyAsync(listEmail);
            return listEmail;
        }
    }
}