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
        public EventRepository(IDbConnection dbConnection)
        {
            _eventCollection = dbConnection.GetConnection<Event>("events");
        }

        public async Task<List<T>> FindById<T>(string id)
        {
            var filtro = Builders<Event>.Filter.Eq("_id", ObjectId.Parse(id));

            var pResults = await _eventCollection
                                            .Find(filtro)
                                            .As<T>()
                                            .ToListAsync();

            return pResults;
        }
    }
}