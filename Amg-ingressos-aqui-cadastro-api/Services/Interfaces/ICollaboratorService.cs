using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Services.Interfaces
{
    public interface ICollaboratorService
    {
        Task<MessageReturn> GetAllCollaboratorOfOrganizerAsync(string idUserOrganizer);
        Task<MessageReturn> GetAllCollaboratorOfEventAssignedAsync(string idEvent, string idUserOrganizer);
        Task<MessageReturn> SendEmailCollaborator(string idEvent, string idUserOrganizer, string link);
    }
}