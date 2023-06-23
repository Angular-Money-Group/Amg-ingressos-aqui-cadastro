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
    public class ProducerColabRepository<T> : IProducerColabRepository
    {
        private readonly IMongoCollection<ProducerColab> _producerColabCollection;

        public ProducerColabRepository(IDbConnection<ProducerColab> dbConnection) {
            this._producerColabCollection = dbConnection.GetConnection("producerXcolab");
        }
        
        public async Task<object> Save<T>(ProducerColab producerColabComplet) {
            try {
                await this._producerColabCollection.InsertOneAsync(producerColabComplet);

                if (producerColabComplet.Id is null)
                    throw new SaveProducerColabException("Erro ao salvar colab x producer do colaborador de id: " + producerColabComplet.IdColab);

                return producerColabComplet.Id;
            }
            catch (SaveProducerColabException ex) {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DoesValueExistsOnField<T>(string fieldName, object value) {
            try {
                var filter = Builders<ProducerColab>.Filter.Eq(fieldName, value);
                var producerColab = await _producerColabCollection.Find(filter).FirstOrDefaultAsync();
                if (producerColab is null)
                    return false;
                return true;
            }   
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ProducerColab> FindByField<T>(string fieldName, object value) {
            try {

                var filter = Builders<ProducerColab>.Filter.Eq(fieldName, value);
                var producerColab = await _producerColabCollection.Find(filter).FirstOrDefaultAsync();
                if (producerColab is not null)
                    return producerColab;
                else
                    throw new ProducerColabNotFound(" não encontrado por " + fieldName + ".");
            }
            catch (ProducerColabNotFound ex) {
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
                var result = await _producerColabCollection.DeleteOneAsync(x => x.Id == id as string);
                if (result.DeletedCount >= 1)
                    return " Deletado.";
                else
                    throw new DeleteProducerColabException("producerXcolab não encontrado.");
            }
            catch (DeleteProducerColabException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ProducerColab>> GetAllProducerColabs<T>() {
            try
            {
                List<ProducerColab> result = await _producerColabCollection.Find(_ => true).ToListAsync();
                if (!result.Any())
                    throw new GetAllProducerColabException("Nenhum producerXcolab encontrado");

                return result;
            }
            catch (GetAllProducerColabException ex)
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