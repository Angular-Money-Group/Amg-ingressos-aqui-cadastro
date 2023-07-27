using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Services.Interfaces
{
    public interface IEmailService
    {
        Task<MessageReturn> SaveAsync(Email email);
        Task<MessageReturn> SaveManyAsync(List<Email> emails);
        MessageReturn Send(string idEmail);
        Task<string> ProcessEmail(List<Email> listEmail);
        string GenerateBody(int randomNumber);
        string GenerateBodyCollaboratorEvent(string link);
        string GenerateBodyLoginColab(User colabInfo, Event eventDetails);
    }
}