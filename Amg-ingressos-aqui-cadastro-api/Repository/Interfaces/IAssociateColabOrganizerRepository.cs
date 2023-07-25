using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Repository.Interfaces
{
    public interface IAssociateColabOrganizerRepository
    {
        Task<object> AssociateColabAsync(AssociateCollaboratorOrganizer associateCollaborator);
        Task<object> AssociateManyColabWithOrganizerAsync(List<AssociateCollaboratorOrganizer> collaboratorOrganizer);
        Task<object> DeleteAssociateColabAsync(string idAssociate);
        Task<object> FindAllColabsOfProducer<T>(string idProducer);
    }
}