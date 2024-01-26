using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Repository.Interfaces
{
    public interface IAssociateColabEventRepository
    {
        Task<AssociateCollaboratorEvent> AssociateCollaboratorEventAsync(AssociateCollaboratorEvent associateCollaborator);
        Task<List<AssociateCollaboratorEvent>> AssociateManyColabWithEventAsync(List<AssociateCollaboratorEvent> collaboratorEvent);
        Task<List<T>> FindAllColabsOfEvent<T>(string idEvent);
        Task<bool> DeleteAssociateCollaboratorEventAsync(string idAssociate);
    }
}