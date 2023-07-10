using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Services.Interfaces
{
    public interface IAssociateService
    {
        Task<MessageReturn> AssociateColabOrganizerAsync(string idUserOrganizer, string idUserColaborator);
        Task<MessageReturn> AssociateColabEventAsync(AssociateColabEvent colabEvent);
    }
}