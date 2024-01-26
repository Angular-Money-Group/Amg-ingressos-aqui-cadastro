using Amg_ingressos_aqui_cadastro_api.Infra;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_cadastro_api.Repository
{
    public class AssociateColabEventRepository : IAssociateColabEventRepository
    {

        private readonly IMongoCollection<AssociateCollaboratorEvent> _associateCollection;
        public AssociateColabEventRepository(IDbConnection dbconnectionIten)
        {
            _associateCollection = dbconnectionIten.GetConnection<AssociateCollaboratorEvent>("event_collaborator");
        }
        public async Task<AssociateCollaboratorEvent> AssociateCollaboratorEventAsync(AssociateCollaboratorEvent associateCollaborator)
        {
            await _associateCollection.InsertOneAsync(associateCollaborator);
            return associateCollaborator;
        }

        public async Task<List<AssociateCollaboratorEvent>> AssociateManyColabWithEventAsync(List<AssociateCollaboratorEvent> collaboratorEvent)
        {
            await _associateCollection.InsertManyAsync(collaboratorEvent);
            return collaboratorEvent;
        }

        public async Task<bool> DeleteAssociateCollaboratorEventAsync(string idAssociate)
        {

            var result = await _associateCollection.DeleteOneAsync(x => x.Id == idAssociate);
            if (result.DeletedCount <= 0)
                return false;

            return true;
        }

        public async Task<List<T>> FindAllColabsOfEvent<T>(string idEvent)
        {
            var filter = Builders<AssociateCollaboratorEvent>.Filter.Eq(x => x.IdEvent, idEvent);
            var eventCollaborator = await _associateCollection.Find(filter)
                                            .As<T>()
                                            .ToListAsync();
            return eventCollaborator;
        }
    }
}