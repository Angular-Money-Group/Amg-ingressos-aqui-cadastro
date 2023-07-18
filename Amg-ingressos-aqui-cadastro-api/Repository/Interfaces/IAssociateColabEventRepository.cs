using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Repository.Interfaces
{
    public interface IAssociateColabEventRepository
    {
        Task<object> AssociateCollaboratorEventAsync(AssociateColabEvent associateCollaborator);
        Task<object> DeleteAssociateCollaboratorEventAsync(string idAssociate);
        Task<object> FindAllColabsOfEvent<T>(string idEvent);
    }
}