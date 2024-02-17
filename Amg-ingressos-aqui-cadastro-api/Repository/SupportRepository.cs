using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Infra;
using MongoDB.Driver;
using Amg_ingressos_aqui_cadastro_api.Enum;
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

        public Task<List<T>> GetAll<T>()
        {
            var results = _supportCollection
                                        .Find(_ => true)
                                        .As<T>()
                                        .ToListAsync();

            if (!results.Result.Any())
            {
                throw new RuleException("Tickets não encontrados");
            }

            return Task.FromResult(results.Result.ToList());
        }

        public Task<T> FindById<T>(string id)
        {
            var filter = Builders<TicketSupport>.Filter.Eq("Id", id);
            var pResults = _supportCollection
                                    .Find(filter)
                                    .As<T>()
                                    .FirstOrDefault();

            return pResults == null
                ? throw new RuleException("Ticket não encontrados")
                : Task.FromResult(pResults);
        }

        public async Task<TicketSupport> Save(TicketSupport ticketSupport)
        {
            await _supportCollection.InsertOneAsync(ticketSupport);

            return ticketSupport;
        }

        public async Task<TicketSupport> EditByIdAsync(string id, TicketSupport ticketSupport)
        {
            var update = Builders<TicketSupport>.Update
                .Set(userMongo => userMongo.Status, ticketSupport.Status)
                .Set(userMongo => userMongo.Message, ticketSupport.Message)
                .Set(userMongo => userMongo.Subject, ticketSupport.Subject);

            var filter = Builders<TicketSupport>.Filter.Eq("Id", id);

            var updateResult = await _supportCollection.UpdateOneAsync(filter, update);
            if (updateResult.MatchedCount > 0)
                return _supportCollection.Find(filter).FirstOrDefault();
            else
                throw new RuleException("Erro ao atualizar usuario.");
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var update = Builders<TicketSupport>.Update.Set(
                userMongo => userMongo.Status,
                StatusSupport.Canceled
            );

            var filter = Builders<TicketSupport>.Filter.Eq("Id", id);

            var updateResult = await _supportCollection.UpdateOneAsync(filter, update);

            if (updateResult.MatchedCount <= 0)
                throw new RuleException("Erro ao deletar usuario.");

            return true;
        }
    }
}