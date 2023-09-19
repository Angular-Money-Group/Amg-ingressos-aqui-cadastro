using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Infra;
using MongoDB.Driver;
using Amg_ingressos_aqui_cadastro_api.Enum;
using System;
using MongoDB.Bson;
using Amg_ingressos_aqui_cadastro_api.Dtos;

namespace Amg_ingressos_aqui_cadastro_api.Repository
{
    public class SupportRepository : ISupportRepository
    {
        private readonly IMongoCollection<TicketSupport> _supportCollection;

        public SupportRepository(IDbConnection<TicketSupport> dbConnection)
        {
            this._supportCollection = dbConnection.GetConnection("ticketsupports");
        }

        public async Task<List<TicketSupport>> GetAll<T>()
        {
            try
            {
                var filter = Builders<TicketSupport>.Filter.Empty;
                var pResults = _supportCollection.Find(filter).ToList();

                if (pResults.Count == 0)
                {
                    throw new GetAllUserException("Tickets não encontrados");
                }

                return pResults;
            }
            catch (GetAllUserException ex)
            {
                throw ex;
            }
        }

        public async Task<TicketSupport> FindById<T>(string id)
        {
            try
            {
                var filter = Builders<TicketSupport>.Filter.Eq("Id", id);
                var pResults = _supportCollection.Find(filter).ToList().FirstOrDefault();

                return pResults == null
                    ? throw new GetAllUserException("Ticket não encontrados")
                    : pResults;
            }
            catch (GetAllUserException ex)
            {
                throw ex;
            }
        }

        public async Task<TicketSupport> Save<T>(TicketSupport ticketSupport)
        {
            try
            {
                await _supportCollection.InsertOneAsync(ticketSupport);

                return ticketSupport;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TicketSupport> UpdateByIdAsync<T>(string id, SupportDTO ticketSupport)
        {
            try
            {
                var update = Builders<TicketSupport>.Update
                    .Set(userMongo => userMongo.Status, ticketSupport.Status ?? StatusSupport.Active)
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
                    throw new UpdateUserException("Erro ao atualizar usuario.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> DeleteAsync<T>(string id)
        {
            try
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
                    throw new DeleteUserException("Erro ao deletar usuario.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
