using Amg_ingressos_aqui_cadastro_api.Infra;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_cadastro_api.Repository
{
    public class AssociateColabEventRepository : IAssociateColabEventRepository
    {

        private readonly IMongoCollection<AssociateCollaboratorEvent> _associateCollection;
        public AssociateColabEventRepository(IDbConnection<AssociateCollaboratorEvent> dbconnectionIten)
        {
            _associateCollection = dbconnectionIten.GetConnection("event_collaborator");
        }
        public async Task<object> AssociateCollaboratorEventAsync(AssociateCollaboratorEvent associateColab)
        {
            try
            {
                await _associateCollection.InsertOneAsync(associateColab);
                return associateColab;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<object> AssociateManyColabWithEventAsync(List<AssociateCollaboratorEvent> collaboratorEvent)
        {
            try
            {
                await _associateCollection.InsertManyAsync(collaboratorEvent);
                return collaboratorEvent;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<object> DeleteAssociateCollaboratorEventAsync(string idAssociate)
        {
            try
            {
                var result = await _associateCollection.DeleteOneAsync(x => x.Id == idAssociate);
                if (result.DeletedCount >= 1)
                    return "Desassociado";
                else
                    throw new Exception("erro ao desassociar colaborador");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<object> FindAllColabsOfEvent<T>(string idEvent)
        {
           try {
                var filter = Builders<AssociateCollaboratorEvent>.Filter.Eq(x=> x.IdEvent, idEvent);
                var eventCollaborator = await _associateCollection.Find(filter)
                                                .ToListAsync();
                return eventCollaborator;
                    
            }
            catch (Exception ex) {
                throw ex;
            }
        }
    }
}