using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Infra;
using MongoDB.Driver;
using Amg_ingressos_aqui_cadastro_api.Enum;
using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Exceptions;

namespace Amg_ingressos_aqui_cadastro_api.Repository
{
    public class SupportRepository : ISupportRepository
    {
        private readonly IMongoCollection<TicketSupport> _supportCollection;

        public SupportRepository(IDbConnection dbConnection)
        {
            _supportCollection = dbConnection.GetConnection<TicketSupport>("ticketsupports");
        }

        public async Task<List<TicketSupport>> GetAll<T>()
        {
            var filter = Builders<TicketSupport>.Filter.Ne(x => x.Status, StatusSupport.Canceled);
            var pResults = _supportCollection.Find(filter).ToList();

            if (pResults.Count == 0)
            {
                throw new RuleException("Tickets não encontrados");
            }

            return pResults;
        }

        public async Task<TicketSupport> FindById<T>(string id)
        {
            var filter = Builders<TicketSupport>.Filter.Eq("Id", id);
            var pResults = _supportCollection.Find(filter).ToList().FirstOrDefault();

            return pResults == null
                ? throw new RuleException("Ticket não encontrados")
                : pResults;
        }

        public async Task<TicketSupport> Save<T>(TicketSupport ticketSupport)
        {
            await _supportCollection.InsertOneAsync(ticketSupport);

            return ticketSupport;
        }

        public async Task<TicketSupport> UpdateByIdAsync<T>(string id, SupportDto ticketSupport)
        {
            var update = Builders<TicketSupport>.Update
                .Set(userMongo => userMongo.Status, ticketSupport.Status)
                .Set(userMongo => userMongo.Message, ticketSupport.Message)
                .Set(userMongo => userMongo.Subject, ticketSupport.Subject);

            var filter = Builders<TicketSupport>.Filter.Eq("Id", id);

            var updateResult = await _supportCollection.UpdateOneAsync(filter, update);
            if (updateResult.MatchedCount > 0)
            {
                return _supportCollection.Find(filter).ToList().FirstOrDefault();
            }
            else
            {
                throw new RuleException("Erro ao atualizar usuario.");
            }
        }

        public async Task<string> DeleteAsync<T>(string id)
        {

            var update = Builders<TicketSupport>.Update.Set(
                userMongo => userMongo.Status,
                StatusSupport.Canceled
            );

            var filter = Builders<TicketSupport>.Filter.Eq("Id", id);

            var updateResult = await _supportCollection.UpdateOneAsync(filter, update);

            if (updateResult.MatchedCount > 0)
            {
                return "Ticket deletado com sucesso.";
            }
            else
            {
                throw new RuleException("Erro ao deletar usuario.");
            }
        }
    }
}
