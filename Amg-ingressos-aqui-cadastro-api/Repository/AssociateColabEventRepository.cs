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
        public async Task<object> AssociateCollaboratorEventAsync(AssociateCollaboratorEvent associateColab)
        {
            await _associateCollection.InsertOneAsync(associateColab);
            return associateColab;
        }

        public async Task<object> AssociateManyColabWithEventAsync(List<AssociateCollaboratorEvent> collaboratorEvent)
        {

            await _associateCollection.InsertManyAsync(collaboratorEvent);
            return collaboratorEvent;
        }

        public async Task<object> DeleteAssociateCollaboratorEventAsync(string idAssociate)
        {

            var result = await _associateCollection.DeleteOneAsync(x => x.Id == idAssociate);
            if (result.DeletedCount >= 1)
                return "Desassociado";
            else
                throw new Exception("erro ao desassociar colaborador");
        }

        public async Task<object> FindAllColabsOfEvent<T>(string idEvent)
        {
            var filter = Builders<AssociateCollaboratorEvent>.Filter.Eq(x => x.IdEvent, idEvent);
            var eventCollaborator = await _associateCollection.Find(filter)
                                            .ToListAsync();
            return eventCollaborator;
        }
    }
}