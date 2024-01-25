using Amg_ingressos_aqui_cadastro_api.Infra;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_cadastro_api.Repository
{
    public class AssociateUserApiDataEventRepository : IAssociateUserApiDataEventRepository
    {
        private readonly IMongoCollection<AssociateUserApiDataEvent> _associateCollection;
        public AssociateUserApiDataEventRepository(IDbConnection<AssociateUserApiDataEvent> dbconnectionIten)
        {
            _associateCollection = dbconnectionIten.GetConnection("apidatauser_event");
        }
        public async Task<object?> AssociateUserApiDataToEventAsync(AssociateUserApiDataEvent data)
        {
            await _associateCollection.InsertOneAsync(data);
            return data;
        }

        public async Task<object?> GetUserApiDataToEventAsync(string idEvent)
        {
            var filter = Builders<AssociateUserApiDataEvent>.Filter.Eq(x => x.IdEvent, idEvent);
            var data = await _associateCollection.Find(filter)
                                            .ToListAsync() ?? throw new Exception("Este produtor ainda não cadastrou nenhum colaborador...");
            return data;
        }
    }
}