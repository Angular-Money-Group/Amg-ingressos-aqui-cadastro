using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Repository.Interfaces
{
    public interface IAssociateColabEventRepository
    {
        Task<object> AssociateCollaboratorEventAsync(AssociateCollaboratorEvent associateCollaborator);
        Task<object> DeleteAssociateCollaboratorEventAsync(string idAssociate);
        Task<object> AssociateManyColabWithEventAsync(List<AssociateCollaboratorEvent> collaboratorEvent);
        Task<object> FindAllColabsOfEvent<T>(string idEvent);
    }
}