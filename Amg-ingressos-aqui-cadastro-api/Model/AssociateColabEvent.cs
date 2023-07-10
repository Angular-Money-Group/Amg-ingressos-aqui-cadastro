using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_cadastro_api.Model
{
    public class AssociateColabEvent
    {
        public string idEvent { get; set; }
        public string idUserColaborator { get; set; }
        public string idUserOrganizer { get; set; }
    }
}