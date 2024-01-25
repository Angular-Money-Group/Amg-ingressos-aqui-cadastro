using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Infra;
using MongoDB.Driver;
using System.Data;
using Amg_ingressos_aqui_cadastro_api.Exceptions;

namespace Amg_ingressos_aqui_cadastro_api.Repository
{
    public class EventColabRepository<T> : IEventColabRepository
    {
        private readonly IMongoCollection<EventColab> _eventColabCollection;

        public EventColabRepository(IDbConnection<EventColab> dbConnection)
        {
            _eventColabCollection = dbConnection.GetConnection("eventXcolab");
        }

        public async Task<object> Save<T>(EventColab eventColabComplet)
        {
            await _eventColabCollection.InsertOneAsync(eventColabComplet);

            if (eventColabComplet.Id is null)
                throw new RuleException("Erro ao salvar colab x event do colaborador de id: " + eventColabComplet.IdColab);

            return eventColabComplet.Id;
        }

        public async Task<bool> DoesValueExistsOnField<T>(string fieldName, object value)
        {
            var filter = Builders<EventColab>.Filter.Eq(fieldName, value);
            var eventColab = await _eventColabCollection.Find(filter).FirstOrDefaultAsync();
            if (eventColab is null)
                return false;
            return true;
        }

        public async Task<EventColab> FindByField<T>(string fieldName, object value)
        {

            var filter = Builders<EventColab>.Filter.Eq(fieldName, value);
            var eventColab = await _eventColabCollection.Find(filter).FirstOrDefaultAsync();
            if (eventColab is not null)
                return eventColab;
            else
                throw new RuleException(" não encontrado por " + fieldName + ".");
        }

        public async Task<object> Delete<T>(object id)
        {

            var result = await _eventColabCollection.DeleteOneAsync(x => x.Id == id as string);
            if (result.DeletedCount >= 1)
                return " Deletado.";
            else
                throw new RuleException("eventXcolab não encontrado.");
        }

        public async Task<List<string>> FindAllColabsOfEvent<T>(string idEvent)
        {
            var filter = Builders<EventColab>.Filter.Eq("IdEvent", idEvent);
            List<EventColab> eventColabs = await _eventColabCollection.Find(filter).ToListAsync();
            List<string> idColabs = new List<string>();
            if (eventColabs is not null)
            {
                foreach (EventColab eventColab in eventColabs)
                {
                    idColabs.Add(eventColab.IdColab);
                }
            }
            return idColabs;
        }
    }
}