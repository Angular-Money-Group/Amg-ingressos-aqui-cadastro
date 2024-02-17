using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Services.Interfaces
{
    public interface IAssociateService
    {
        Task<MessageReturn> AssociateColabOrganizerAsync(string idUserOrganizer, UserDto user);
        Task<MessageReturn> AssociateManyColabWithOrganizerAsync(List<AssociateCollaboratorOrganizer> colaboratorOrganizer);
        Task<MessageReturn> AssociateCollaboratorEventAsync(AssociateCollaboratorEvent collaboratorEvent);
        Task<MessageReturn> AssociateManyColabWithEventAsync(List<AssociateCollaboratorEvent> collaboratorEvent);
        Task<MessageReturn> DeleteAssociateColabOrganizerAsync(string idAssociate);
        Task<MessageReturn> DeleteAssociateColabEventAsync(string idAssociate);
        Task<MessageReturn> AssociateUserApiDataToEventAsync(string idEvent, string idUser);
        Task<MessageReturn> GetUserApiDataToEventAsync(string idEvent);
    }
}