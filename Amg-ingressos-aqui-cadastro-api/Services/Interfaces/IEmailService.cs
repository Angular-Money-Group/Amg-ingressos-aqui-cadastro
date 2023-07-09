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
        MessageReturn Send(string idEmail);
        string GenerateBody();
    }
}