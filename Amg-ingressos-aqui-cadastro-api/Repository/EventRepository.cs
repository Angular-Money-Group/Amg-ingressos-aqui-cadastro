

using Amg_ingressos_aqui_cadastro_api.Infra;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_cadastro_api.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly IMongoCollection<Event> _eventCollection;
       public EventRepository(IDbConnection<Event> dbConnection) {
            this._eventCollection = dbConnection.GetConnection("events");
        }

        public async Task<List<Event>> FindById<T>(string id)
        {
            try
            {
                var filtro = Builders<Event>.Filter.Eq("_id", ObjectId.Parse(id));

                List<Event> pResults = await _eventCollection.Find<Event>(filtro).ToListAsync();

                return pResults;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}