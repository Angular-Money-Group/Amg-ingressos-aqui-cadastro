using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using System.Diagnostics.CodeAnalysis;
using Amg_ingressos_aqui_cadastro_api.Infra;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_cadastro_api.Repository
{
    public class EventColabRepository<T> : IEventColabRepository
    {
        private readonly IMongoCollection<EventColab> _eventColabCollection;

        public EventColabRepository(IDbConnection<EventColab> dbConnection) {
            this._eventColabCollection = dbConnection.GetConnection("eventXcolab");
        }
        
        public async Task<object> Save<T>(EventColab eventColabComplet) {
            try {
                await this._eventColabCollection.InsertOneAsync(eventColabComplet);

                if (eventColabComplet.Id is null)
                    throw new SaveEventColabException("Erro ao salvar colab x event do colaborador de id: " + eventColabComplet.IdColab);

                return eventColabComplet.Id;
            }
            catch (SaveEventColabException ex) {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DoesValueExistsOnField<T>(string fieldName, object value) {
            try {
                var filter = Builders<EventColab>.Filter.Eq(fieldName, value);
                var eventColab = await _eventColabCollection.Find(filter).FirstOrDefaultAsync();
                if (eventColab is null)
                    return false;
                return true;
            }   
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EventColab> FindByField<T>(string fieldName, object value) {
            try {

                var filter = Builders<EventColab>.Filter.Eq(fieldName, value);
                var eventColab = await _eventColabCollection.Find(filter).FirstOrDefaultAsync();
                if (eventColab is not null)
                    return eventColab;
                else
                    throw new EventColabNotFound(" não encontrado por " + fieldName + ".");
            }
            catch (EventColabNotFound ex) {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public async Task<object> Delete<T>(object id) {
            try
            {
                var result = await _eventColabCollection.DeleteOneAsync(x => x.Id == id as string);
                if (result.DeletedCount >= 1)
                    return " Deletado.";
                else
                    throw new DeleteEventColabException("eventXcolab não encontrado.");
            }
            catch (DeleteEventColabException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<EventColab>> GetAllEventColabs<T>() {
            try
            {
                List<EventColab> result = await _eventColabCollection.Find(_ => true).ToListAsync();
                if (!result.Any())
                    throw new GetAllEventColabException("Nenhum eventXcolab encontrado");

                return result;
            }
            catch (GetAllEventColabException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}