using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Infra;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_cadastro_api.Repository
{
    public class AssociateColabOrganizerRepository : IAssociateColabOrganizerRepository
    {
        private readonly IMongoCollection<AssociateCollaboratorOrganizer> _associateCollection;
        public AssociateColabOrganizerRepository(IDbConnection dbconnectionIten)
        {
            _associateCollection = dbconnectionIten.GetConnection<AssociateCollaboratorOrganizer>("organizer_colaborator");
        }
        public async Task<object> AssociateColabAsync(AssociateCollaboratorOrganizer associateColab)
        {
            await _associateCollection.InsertOneAsync(associateColab);
            return associateColab;
        }

        public async Task<object> AssociateManyColabWithOrganizerAsync(List<AssociateCollaboratorOrganizer> collaboratorOrganizer)
        {
            await _associateCollection.InsertManyAsync(collaboratorOrganizer);
            return collaboratorOrganizer;
        }

        public async Task<object> DeleteAssociateColabAsync(string idAssociate)
        {
            var result = await _associateCollection.DeleteOneAsync(x => x.Id == idAssociate);
            if (result.DeletedCount >= 1)
                return "Desassociado";
            else
                throw new Exception("erro ao desassociar colaborador");
        }

        public async Task<List<T>> FindAllColabsOfProducer<T>(string idUserOrganizer)
        {

            var filter = Builders<AssociateCollaboratorOrganizer>.Filter.Eq(x => x.IdUserOrganizer, idUserOrganizer);
            var producerColabs = await _associateCollection.Find(filter)
                                            .As<T>()
                                            .ToListAsync() ??
                                            throw new RuleException("Este produtor ainda não cadastrou nenhum colaborador...");
            return producerColabs;
        }
    }
}