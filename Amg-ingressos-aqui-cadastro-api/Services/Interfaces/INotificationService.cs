using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Services.Interfaces
{
    public interface INotificationService
    {
        Task<MessageReturn> SaveAsync<T>(T email);
        Task<MessageReturn> ProcessEmail(List<EmailLoginCollaboratorCredentialDto> listEmail);
    }
}