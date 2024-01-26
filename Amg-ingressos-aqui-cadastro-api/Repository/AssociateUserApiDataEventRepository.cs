using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Infra;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_cadastro_api.Repository
{
    public class AssociateUserApiDataEventRepository : IAssociateUserApiDataEventRepository
    {
        private readonly IMongoCollection<AssociateUserApiDataEvent> _associateCollection;
        public AssociateUserApiDataEventRepository(IDbConnection dbconnectionIten)
        {
            _associateCollection = dbconnectionIten.GetConnection<AssociateUserApiDataEvent>("apidatauser_event");
        }
        public async Task<AssociateUserApiDataEvent> AssociateUserApiDataToEventAsync(AssociateUserApiDataEvent data)
        {
            await _associateCollection.InsertOneAsync(data);
            return data;
        }

        public async Task<List<T>> GetUserApiDataToEventAsync<T>(string idEvent)
        {
            var filter = Builders<AssociateUserApiDataEvent>.Filter.Eq(x => x.IdEvent, idEvent);
            var data = await _associateCollection.Find(filter)
                                            .As<T>()
                                            .ToListAsync() ?? throw new RuleException("Este produtor ainda não cadastrou nenhum colaborador...");
            return data;
        }
    }
}