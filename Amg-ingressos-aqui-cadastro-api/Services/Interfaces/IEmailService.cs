using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Services.Interfaces
{
    public interface IEmailService
    {
        Task<MessageReturn> SaveAsync(object email);
        Task<string> ProcessEmail(List<EmailLoginCollaboratorCredentialDto> listEmail);
    }
}