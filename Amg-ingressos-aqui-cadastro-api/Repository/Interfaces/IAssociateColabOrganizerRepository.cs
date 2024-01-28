using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Repository.Interfaces
{
    public interface IAssociateColabOrganizerRepository
    {
        Task<AssociateCollaboratorOrganizer> AssociateColabAsync(AssociateCollaboratorOrganizer associateCollaborator);
        Task<List<AssociateCollaboratorOrganizer>> AssociateManyColabWithOrganizerAsync(List<AssociateCollaboratorOrganizer> collaboratorOrganizer);
        Task<bool> DeleteAssociateColabAsync(string idAssociate);
        Task<List<T>> GetAllColabsOfProducer<T>(string idProducer);
    }
}