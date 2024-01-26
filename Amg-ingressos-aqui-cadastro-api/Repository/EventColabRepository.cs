using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Infra;
using MongoDB.Driver;
using Amg_ingressos_aqui_cadastro_api.Exceptions;

namespace Amg_ingressos_aqui_cadastro_api.Repository
{
    public class EventColabRepository : IEventColabRepository
    {
        private readonly IMongoCollection<EventColab> _eventColabCollection;

        public EventColabRepository(IDbConnection dbConnection)
        {
            _eventColabCollection = dbConnection.GetConnection<EventColab>("eventXcolab");
        }

        public async Task<EventColab> Save(EventColab eventColabComplet)
        {
            await _eventColabCollection.InsertOneAsync(eventColabComplet);

            if (eventColabComplet.Id is null)
                throw new RuleException("Erro ao salvar colab x event do colaborador de id: " + eventColabComplet.IdColab);

            return eventColabComplet;
        }

        public async Task<bool> DoesValueExistsOnField<T>(string fieldName, object value)
        {
            var filter = Builders<EventColab>.Filter.Eq(fieldName, value);
            var eventColab = await _eventColabCollection.Find(filter).FirstOrDefaultAsync();
            if (eventColab is null)
                return false;
            return true;
        }

        public async Task<T> FindByField<T>(string fieldName, object value)
        {

            var filter = Builders<EventColab>.Filter.Eq(fieldName, value);
            var eventColab = await _eventColabCollection
                                        .Find(filter)
                                        .As<T>()
                                        .FirstOrDefaultAsync();
            if (eventColab == null)
                throw new RuleException(" não encontrado por " + fieldName + ".");
            return eventColab;
        }

        public async Task<bool> Delete(object id)
        {

            var result = await _eventColabCollection.DeleteOneAsync(x => x.Id == id as string);
            if (result.DeletedCount <= 0)
                throw new RuleException("eventXcolab não encontrado.");
            return true;
        }

        public async Task<List<string>> FindAllColabsOfEvent(string idEvent)
        {
            var filter = Builders<EventColab>.Filter.Eq("IdEvent", idEvent);
            List<EventColab> eventColabs = await _eventColabCollection.Find(filter).ToListAsync();
            List<string> idColabs = new List<string>();
            if (eventColabs is not null)
                idColabs.AddRange(eventColabs.Select(x => x.IdColab).ToList());
            return idColabs;
        }
    }
}