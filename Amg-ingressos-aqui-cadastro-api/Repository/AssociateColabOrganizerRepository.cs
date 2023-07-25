using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Infra;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_cadastro_api.Repository
{
    public class AssociateColabOrganizerRepository: IAssociateColabOrganizerRepository
    {
        private readonly IMongoCollection<AssociateCollaboratorOrganizer> _associateCollection;
        public AssociateColabOrganizerRepository(IDbConnection<AssociateCollaboratorOrganizer> dbconnectionIten)
        {
            _associateCollection = dbconnectionIten.GetConnection("organizer_colaborator");
        }
        public async Task<object> AssociateColabAsync(AssociateCollaboratorOrganizer associateColab)
        {
            try
            {
                await _associateCollection.InsertOneAsync(associateColab);
                return associateColab;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<object> AssociateManyColabWithOrganizerAsync(List<AssociateCollaboratorOrganizer> collaboratorOrganizer)
        {
            try
            {
                await _associateCollection.InsertManyAsync(collaboratorOrganizer);
                return collaboratorOrganizer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<object> DeleteAssociateColabAsync(string idAssociate){
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
                throw ex;
            }
        }

        public async Task<object> FindAllColabsOfProducer<T>(string idUserOrganizer) {
            try {

                var filter = Builders<AssociateCollaboratorOrganizer>.Filter.Eq(x=> x.IdUserOrganizer, idUserOrganizer);
                var producerColabs = await _associateCollection.Find(filter)
                                                .ToListAsync() ?? 
                                                throw new ProducerColabNotFound("Este produtor ainda não cadastrou nenhum colaborador...");
                return producerColabs; 
            }
            catch (ProducerColabNotFound ex) {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}