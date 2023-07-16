using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Repository.Interfaces
{
    public interface IAssociateColabEventRepository
    {
        Task<Object> AssociateColabEventAsync(AssociateColabEvent associateColab);
        Task<object> FindAllColabsOfEvent<T>(string idEvent);
    }
}