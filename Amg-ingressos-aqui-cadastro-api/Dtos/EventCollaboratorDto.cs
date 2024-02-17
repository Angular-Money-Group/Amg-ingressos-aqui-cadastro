using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Dtos
{
    public class EventCollaboratorDto : EventColab
    {
        public EventColab MakeEventColab()
        {
            return new EventColab();
        }
    }
}